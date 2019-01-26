using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TimeSerie.ExternalReader.ChmiAimData
{
    public class ChmiAimDataDownloader
    {
        public async Task<Stream> Download(string p_Uri)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(p_Uri);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Response ERROR: {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
