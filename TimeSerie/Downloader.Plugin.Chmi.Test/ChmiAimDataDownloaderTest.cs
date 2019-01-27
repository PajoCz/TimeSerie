using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Downloader.Plugin.ChmiAimData.Test
{
    public class ChmiAimDataDownloaderTest
    {
        [Fact]
        public async Task Test()
        {
            var uri = "http://portal.chmi.cz/files/portal/docs/uoco/web_generator/AIMdata_hourly.xml";
            using (var fs = File.OpenWrite($"AIMdata-{DateTime.Now:yyyy-MM-dd hh-mm-ss}.xml"))
            {
                var stream = await new ChmiAimDataDownloader().Download(uri);
                await stream.CopyToAsync(fs);
            }
        }
    }
}
