using LazyFetcher.Interface;
using System;
using System.IO;
using System.Linq;
using LazyFetcher.Data;

namespace LazyFetcher
{
    public class DefaultFeedManager : IFeedManager
    {
        private readonly IMessenger _messenger;
        private readonly IUrlFetcher _urlFetcher;
        private readonly IOptions _options;
        private readonly ILeague _league;
        private readonly IDownloader _downloader;
        private readonly IProxy _proxy;

        private DateTime _startDate;
        private DateTime _endDate;

        public DefaultFeedManager(IMessenger messenger, IUrlFetcher urlFetcher, IOptions options,
            IProxy proxy, IDownloader downloader, ILeague league)
        {
            _messenger = messenger;
            _urlFetcher = urlFetcher;
            _options = options;
            _proxy = proxy;
            _league = league;
            _downloader = downloader;

            _startDate = _endDate = (options.Date != null) ? DateTime.Parse(options.Date) : DateTime.Now;
            _startDate = _startDate.Subtract(new TimeSpan(24 * options.Days, 0, 0));            
        }
        public void ChooseFeed(string targetPath, bool getOnlyUrl)
        {            
            var feeds = _urlFetcher.GetFeeds(_startDate, _endDate);

            foreach (var feed in feeds)
            {
                Console.WriteLine($"{feed.Id.ToString().PadLeft(2)}: {feed.Date} {feed.Away}@{feed.Home} {feed.Type} ({feed.Name})");
            }

            if (!feeds.Any())
            {
                _messenger.WriteLine("No feeds were found.");
                return;
            }
            
            Console.Write("\nChoose feed (q to quit): ");
            var input = Console.ReadLine();

            if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            
            var chosenFeed = feeds.FirstOrDefault(f => f.Id == int.Parse(input));
            var streamUrl = _urlFetcher.GetStreamUrl(chosenFeed);

            if (getOnlyUrl)
            {
                Console.WriteLine(streamUrl);
                return;
            }

            _messenger.WriteLine($"\nDownloading feed: {chosenFeed.Date} {chosenFeed.Away}@{chosenFeed.Home} ({chosenFeed.Type},{chosenFeed.Name})");

            Download(streamUrl, chosenFeed, targetPath);
        }

        public void GetLatest(string teamName, string targetPath, bool getOnlyUrl)
        {
            if (!getOnlyUrl)
            {
                _messenger.WriteLine($"Fetching latest feed for '{teamName}'...\n");
            }                

            var feed = _urlFetcher.GetLatestFeedForTeam(teamName, _startDate, _endDate);            
            var streamUrl = _urlFetcher.GetStreamUrl(feed);

            if (getOnlyUrl)
            {
                Console.WriteLine(streamUrl);
                return;
            }

            _messenger.WriteLine($"Feed found: {feed.Date} {feed.Away}@{feed.Home} ({feed.Type},{feed.Name})");

            Download(streamUrl, feed, targetPath);
        }               

        private void Download(string streamUrl, Feed feed, string targetPath)
        {
            var fileName = GetTargetFileName(feed, targetPath);            

            if (File.Exists(fileName) && !_options.OverwriteExistingFile)
            {
                _messenger.WriteLine("Skipping download because file already exists.");
                return;
            }
            
            try
            {
                if (_options.UseProxy && _league.IsRedirectionRequired)
                {                    
                    if (!_proxy.Start())
                    {
                        return;
                    }
                }
                var downloadRequest = new DownloadRequest() { Proxy = _proxy, StreamUrl = streamUrl, TargetFileName = fileName };
                _downloader.Download(downloadRequest);
            }
            finally
            {
                if (_proxy != null)
                {
                    _proxy.Stop();
                }
            }
        }

        private string GetTargetFileName(Feed feed, string directoryPath)
        {
            return Path.Combine(directoryPath ?? "", $"{feed.Date}-{feed.Away}@{feed.Home}-{feed.Name}.mp4");
        }
    }
}
