using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeSerie.Core.Domain
{
    public static class TimeSerieHeaderExtensions
    {
        public static TimeSerieHeader FindWithSameProperties(this IEnumerable<TimeSerieHeader> p_This,
            ICollection<TimeSerieHeaderProperty> p_Properties)
        {
            var result = p_This.ToList().Find(tsdb =>
            {
                foreach (var dbProp in tsdb.TimeSerieHeaderProperties)
                {
                    var propSameNameValueFound = p_Properties.ToList()
                        .Find(strProp => strProp.Name == dbProp.Name && strProp.Value == dbProp.Value);
                    if (propSameNameValueFound == null) return false;
                }

                return true;
            });

            return result;
        }

        public static string ToSortedJoinedString(this IEnumerable<TimeSerieHeaderProperty> p_This, string p_Delimited = ", ")
        {
            var thisSorted = p_This.OrderBy(i => i.Name);
            StringBuilder sb = new StringBuilder();
            foreach (var i in thisSorted.SkipLast(1))
            {
                sb.Append($"{i.Name}={i.Value}{p_Delimited}");
            }
            sb.Append($"{thisSorted.Last().Name}={thisSorted.Last().Value}");
            return sb.ToString();
        }
    }
}