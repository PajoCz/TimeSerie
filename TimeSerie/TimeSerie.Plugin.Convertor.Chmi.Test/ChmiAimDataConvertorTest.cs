using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimeSerie.Core.Domain;
using Xunit;

namespace TimeSerie.Plugin.Convertor.Chmi.Test
{
    public class ChmiAimDataConvertorTest
    {
        [Fact]
        public async Task ConvertTest()
        {
            using (var fs = File.OpenRead("Files\\AIMdata-2019-01-26 18-00 2 stations.xml"))
            {
                var result = (await new ChmiAimDataConvertor().Convert(fs)).ToList();
                Assert.Equal(4, result.Count);

                Assert.Equal(TimeSerieType.Decimal, result[0].TimeSerieType);
                Assert.Null(result[0].ValueStrings);
                Assert.Equal(1, result[0].ValueDecimals.Count);
                Assert.Equal(new DateTime(2019,1,26,18,0,0), result[0].ValueDecimals.First().DateTimeOffset.DateTime);
                Assert.Equal(14.9m, result[0].ValueDecimals.First().Value);

                Assert.Equal(5, result[0].TimeSerieHeaderProperties.Count);
                Assert.Equal("code=ABREA, component=NO2, latitude=50.084385, longtitude=14.380116, name=Praha 6-Bøevnov", result[0].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=14,9", result[0].ValueDecimals.ToSortedJoinedString());
                Assert.Equal("code=ABREA, component=PM10, latitude=50.084385, longtitude=14.380116, name=Praha 6-Bøevnov", result[1].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=5", result[1].ValueDecimals.ToSortedJoinedString());
                Assert.Equal("code=ACHOA, component=NO2, latitude=50.030172, longtitude=14.51745, name=Praha 4-Chodov", result[2].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=17,2", result[2].ValueDecimals.ToSortedJoinedString());
                Assert.Equal("code=ACHOA, component=PM10, latitude=50.030172, longtitude=14.51745, name=Praha 4-Chodov", result[3].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=22", result[3].ValueDecimals.ToSortedJoinedString());
            }
        }
    }
}
