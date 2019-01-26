using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeSerie.Core.Domain
{
    public class TimeSerieValue<T>
    {
        public TimeSerieValue(DateTimeOffset dateTimeOffset, T value)
        {
            DateTimeOffset = dateTimeOffset;
            Value = value;
        }

        [Key]
        public long TimeSerieValueBaseId { get; set; }
        public int TimeSerieHeaderId { get; set; }
        public TimeSerieHeader TimeSerieHeader { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public T Value { get; set; }
    }
}
