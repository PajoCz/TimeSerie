using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSerie.Core.Domain
{
    public class TimeSerieValueDecimal : TimeSerieValue<decimal>
    {
        public TimeSerieValueDecimal(DateTimeOffset dateTimeOffset, decimal value) : base(dateTimeOffset, value)
        {
        }
    }
}
