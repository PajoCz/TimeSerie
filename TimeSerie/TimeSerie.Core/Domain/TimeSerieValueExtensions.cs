using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeSerie.Core.Domain
{
    public static class TimeSerieValueExtensions
    {
        public static string ToSortedJoinedString<T>(this IEnumerable<TimeSerieValue<T>> p_This, string p_Delimited = ", ")
        {
            var thisSorted = p_This.OrderBy(i => i.DateTimeOffset);
            StringBuilder sb = new StringBuilder();
            foreach (var i in thisSorted.SkipLast(1))
            {
                sb.Append($"{i.DateTimeOffset}={i.Value}{p_Delimited}");
            }
            sb.Append($"{thisSorted.Last().DateTimeOffset}={thisSorted.Last().Value}");
            return sb.ToString();

        }
    }
}
