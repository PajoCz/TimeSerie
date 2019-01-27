using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSerie.Core.Domain
{
    public class TimeSerieValueString : TimeSerieValue<string>
    {
        public TimeSerieValueString(DateTimeOffset dateTimeOffset, string value) : base(dateTimeOffset, value)
        {
        }
    }
}
