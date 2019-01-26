using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TimeSerie.ExternalReader.ChmiAimData.Test
{
    public class ChmiAimDataDownloaderTest
    {
        [Fact]
        public async Task Test()
        {
            var uri = "http://portal.chmi.cz/files/portal/docs/uoco/web_generator/AIMdata_hourly.xml";
            using (var fs = File.OpenWrite("AIMdata.xml"))
            {
                var stream = await new ChmiAimDataDownloader().Download(uri);
                await stream.CopyToAsync(fs);
            }
        }
    }
}
