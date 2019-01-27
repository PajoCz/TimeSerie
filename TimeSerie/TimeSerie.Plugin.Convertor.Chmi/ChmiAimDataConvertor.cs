using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using TimeSerie.Core.Convertor;
using TimeSerie.Core.Domain;

namespace TimeSerie.Plugin.Convertor.Chmi
{
    public class ChmiAimDataConvertor : ITimeSerieConvertor
    {
        public Task<IEnumerable<TimeSerieHeader>> Convert(Stream p_Stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(p_Stream);
            var datetimetoUtc = DateTime.Parse(doc.SelectSingleNode("/AQ_hourly_index/Data/datetime_to").InnerText);
            var datetimefromUtc = datetimetoUtc.AddHours(-1);
            var datetimefromLocal = datetimefromUtc.ToLocalTime();
            List<TimeSerieHeader> result = new List<TimeSerieHeader>();
            foreach(XmlNode stationNode in doc.SelectNodes("/AQ_hourly_index/Data/station"))
            {
                string component = null;
                string value = null;

                var measurmentNode = stationNode.SelectSingleNode("measurement");
                if (measurmentNode != null)
                {
                    foreach (XmlNode node in measurmentNode.ChildNodes)
                    {
                        if (node.Name == "component")
                        {
                            component = node.InnerText;
                        }
                        else if (node.Name == "averaged_time" &&
                                 node.SelectSingleNode("averaged_hours").InnerText == "1")
                        {
                            value = node.SelectSingleNode("value").InnerText;

                            result.Add(new TimeSerieHeader()
                            {
                                TimeSerieType = TimeSerieType.Decimal,
                                TimeSerieHeaderProperties = new List<TimeSerieHeaderProperty>()
                                {
                                    new TimeSerieHeaderProperty("code", stationNode.SelectSingleNode("code").InnerText),
                                    new TimeSerieHeaderProperty("name", stationNode.SelectSingleNode("name").InnerText),
                                    new TimeSerieHeaderProperty("longtitude", stationNode.SelectSingleNode("wgs84_longitude").InnerText),
                                    new TimeSerieHeaderProperty("latitude", stationNode.SelectSingleNode("wgs84_latitude").InnerText),
                                    new TimeSerieHeaderProperty("component", component)
                                },
                                ValueDecimals = new List<TimeSerieValueDecimal>()
                                {
                                    new TimeSerieValueDecimal(datetimefromLocal,
                                        decimal.Parse(value, CultureInfo.GetCultureInfo("en-EN")))
                                }
                            });
                            //result.Add(new TimeSerieHeader()
                            //{
                            //    TimeSerieType = TimeSerieType.String,
                            //    TimeSerieHeaderProperties = new List<TimeSerieHeaderProperty>()
                            //    {
                            //        new TimeSerieHeaderProperty("code", stationNode.SelectSingleNode("code").InnerText),
                            //        new TimeSerieHeaderProperty("name", stationNode.SelectSingleNode("name").InnerText),
                            //        new TimeSerieHeaderProperty("longtitude", stationNode.SelectSingleNode("wgs84_longitude").InnerText),
                            //        new TimeSerieHeaderProperty("latitude", stationNode.SelectSingleNode("wgs84_latitude").InnerText),
                            //        new TimeSerieHeaderProperty("component", component)
                            //    },
                            //    ValueStrings = new List<TimeSerieValue<string>>()
                            //    {
                            //        new TimeSerieValue<string>(datetimefromLocal, value)
                            //    }
                            //});
                        }
                    }
                }
            }

            return Task.FromResult(result as IEnumerable<TimeSerieHeader>);
        }
    }
}
