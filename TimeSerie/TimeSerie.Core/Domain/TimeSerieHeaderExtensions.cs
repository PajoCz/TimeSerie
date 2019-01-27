using System.Collections.Generic;
using System.Linq;

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
    }
}