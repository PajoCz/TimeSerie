using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace TimeSerie.Service.Test
{
    public class TimeSerieHeaderServiceTest
    {
        [Fact]
        public async Task Test1()
        {
            using (var fs = File.OpenRead("AIMdata.xml"))
            {
                await new TimeSerieHeaderService().Process(fs);
            }
        }
    }
}
