
namespace LazyFetcher.Data
{
    public class DownloadRequest
    {
        public string StreamUrl { get; set; }
        public string TargetFileName { get; set; }
        public bool IsProxyRequired { get; set; }
        public int ProxyPort { get; set; }
    }
}
