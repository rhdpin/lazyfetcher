﻿
using LazyFetcher.Interface;

namespace LazyFetcher.Data
{
    public class DownloadRequest
    {
        public string StreamUrl { get; set; }
        public string TargetFileName { get; set; }
        public bool UseProxy { get; set; }
    }
}
