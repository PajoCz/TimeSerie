using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeSerie.Core.Domain
{
    public class TimeSerieHeaderProperty
    {
        public TimeSerieHeaderProperty()
        {
        }

        public TimeSerieHeaderProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [Key]
        public int TimeSerieHeaderPropertyId { get; set; }
        public int TimeSerieHeaderId { get; set; }
        public TimeSerieHeader TimeSerieHeader { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
