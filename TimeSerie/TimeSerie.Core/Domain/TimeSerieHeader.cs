using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeSerie.Core.Domain
{
    public class TimeSerieHeader
    {
        [Key]
        public int TimeSerieHeaderId { get; set; }
        //public Guid Identifier { get; set; }
        //TODO: json metadata (not only Name) with easy searching
        //public string Name { get; set; }
        public ICollection<TimeSerieValue<decimal>> ValueDecimals { get; set; }
        public ICollection<TimeSerieValue<string>> ValueStrings { get; set; }
        public TimeSerieType TimeSerieType { get; set; }
        //todo: redundant info serialized or calculated from ValueDecimals/ValueStrings by TimeSerieType
        public DateTimeOffset? ValuesFrom { get; set; }
        public DateTimeOffset? ValuesTo { get; set; }
    }
}
