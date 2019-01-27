using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Downloader.Plugin.ChmiAimData;

namespace TimeSerie.AspNetApp.Code.Scheduling
{
    public class DownloaderChmiScheduledTask : IScheduledTask
    {
        public string Schedule => "* * * * *";
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var stream = await new ChmiAimDataDownloader().Download("http://portal.chmi.cz/files/portal/docs/uoco/web_generator/AIMdata_hourly.xml");
            using (var f = File.OpenWrite($"c:\\Users\\pajo\\Source\\Repos\\TimeSerie\\Data\\DownloaderChmi{DateTime.Now:yyyy-MM-dd HH-mm}.xml"))
            {
                await stream.CopyToAsync(f, cancellationToken);
            }
        }
    }
}
