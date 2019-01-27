using Downloader.Core;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Downloader.Plugin.ChmiAimData
{
    public class ChmiAimDataDownloader : IDownloader
    {
        public async Task<Stream> Download(string p_Path)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(p_Path);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Response ERROR: {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
