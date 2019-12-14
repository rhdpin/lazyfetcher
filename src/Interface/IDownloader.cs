using LazyFetcher.Data;
using System;

namespace LazyFetcher.Interface
{
    public interface IDownloader : IDisposable
    {        
        void Download(DownloadRequest request);
    }
}
