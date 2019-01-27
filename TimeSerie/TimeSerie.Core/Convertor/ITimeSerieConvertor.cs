using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TimeSerie.Core.Domain;

namespace TimeSerie.Core.Convertor
{
    public interface ITimeSerieConvertor
    {
        Task<IEnumerable<TimeSerieHeader>> Convert(Stream p_Stream);
    }
}
