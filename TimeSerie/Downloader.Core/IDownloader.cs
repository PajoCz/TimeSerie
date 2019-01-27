using System;
using System.IO;
using System.Threading.Tasks;

namespace Downloader.Core
{
    public interface IDownloader
    {
        Task<Stream> Download(string p_Path);
    }
}
