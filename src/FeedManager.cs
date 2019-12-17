using LazyFetcher.Interface;
using System;
using System.IO;
using LightInject;
using System.Linq;
using LazyFetcher.Data;

namespace LazyFetcher
{
    public class FeedManager 
    {
        private DateTime _startDate;
        private DateTime _endDate;
        public FeedManager()
        {
            // Uses fixed time period for now
            _startDate = DateTime.Now.Subtract(new TimeSpan(48, 0, 0));
            _endDate = DateTime.Now;            
        }
        public void ChooseFeed(string targetPath, bool getOnlyUrl)
        {
            var urlFetcher = Program.IocContainer.GetInstance<IUrlFetcher>();
            var feeds = urlFetcher.GetFeeds(_startDate, _endDate);

            foreach (var feed in feeds)
            {
                Console.WriteLine($"{feed.Id.ToString().PadLeft(2)}: {feed.Date} {feed.Away}@{feed.Home} {feed.Type} ({feed.Name})");
            }

            Console.Write("\nChoose feed (q to quit): ");
            var input = Console.ReadLine();

            if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var chosenFeed = feeds.FirstOrDefault(f => f.Id == int.Parse(input));
            var streamUrl = urlFetcher.GetStreamUrl(chosenFeed);

            if (getOnlyUrl)
            {
                Console.WriteLine(streamUrl);
                return;
            }

            Console.WriteLine($"\nDownloading feed: {chosenFeed.Date} {chosenFeed.Away}@{chosenFeed.Home} ({chosenFeed.Type},{chosenFeed.Name})");

            Download(streamUrl, chosenFeed, targetPath);
        }

        public void GetLatest(string teamName, string targetPath, bool getOnlyUrl)
        {
            if (!getOnlyUrl)
            {
                Console.Write($"Fetching latest feed for '{teamName}'...\n");
            }                

            var urlFetcher = Program.IocContainer.GetInstance<IUrlFetcher>();
            var feed = urlFetcher.GetLatestFeedForTeam(teamName, _startDate, _endDate);            
            var streamUrl = urlFetcher.GetStreamUrl(feed);

            if (getOnlyUrl)
            {
                Console.WriteLine(streamUrl);
                return;
            }

            Console.WriteLine($"Feed found: {feed.Date} {feed.Away}@{feed.Home} ({feed.Type},{feed.Name})");

            Download(streamUrl, feed, targetPath);
        }               

        private void Download(string streamUrl, Feed feed, string targetPath)
        {
            var fileName = GetTargetFileName(feed, targetPath);
            var league = Program.IocContainer.GetInstance<ILeague>();
            IProxy proxy = null;
            try
            {
                if (league.IsProxyRequired)
                {
                    proxy = Program.IocContainer.GetInstance<IProxy>();
                    if (!proxy.Start())
                    {
                        return;
                    }
                }
                var downloadRequest = new DownloadRequest() { IsProxyRequired = league.IsProxyRequired, ProxyPort = proxy.Port, StreamUrl = streamUrl, TargetFileName = fileName };
                var downloader = Program.IocContainer.GetInstance<IDownloader>();
                downloader.Download(downloadRequest);
            }
            finally
            {
                if (proxy != null)
                {
                    proxy.Stop();
                }
            }
        }

        private string GetTargetFileName(Feed feed, string directoryPath)
        {
            return Path.Combine(directoryPath ?? "", $"{feed.Date}-{feed.Away}@{feed.Home}-{feed.Name}.mp4");
        }
    }
}
