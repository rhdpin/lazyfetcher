using LazyFetcher.Data;
using LazyFetcher.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LazyFetcher.NHL
{
    public class NHLUrlFetcher : IUrlFetcher
    {
        private IMessenger _messenger;

        public NHLUrlFetcher(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public IEnumerable<Feed> GetFeeds(DateTime startDate, DateTime endDate)
        {
            var model = GetModel(startDate, endDate);
            return GetFeeds(model);
        }

        public Feed GetLatestFeedForTeam(string teamName, DateTime startDate, DateTime endDate)
        {
            _messenger.WriteLine($"Getting latest feed for {teamName} for game played between {startDate} and {endDate}", Messenger.MessageCategory.Verbose);

            var model = GetModel(startDate, endDate);
            var feeds = GetFeeds(model);

            var feedsWithSelectedTeam = feeds.OrderByDescending(f => f.Date)
                .Where(f => f.Away.Equals(teamName, StringComparison.OrdinalIgnoreCase) ||
                f.Home.Equals(teamName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (feedsWithSelectedTeam.Count == 0)
            {
                _messenger.WriteLine($"No game for '{teamName}' was found ({startDate}-{endDate})");
                return null;
            }

            var latestDate = feedsWithSelectedTeam[0].Date;
            var isHomeTeam = feedsWithSelectedTeam[0].Home.Equals(teamName, StringComparison.OrdinalIgnoreCase);
            var latestStreams = feedsWithSelectedTeam.Where(f => f.Date.Equals(latestDate));

            Feed chosenFeed;
            if (isHomeTeam)
                chosenFeed = latestStreams.FirstOrDefault(f => f.Type.Equals("home", StringComparison.OrdinalIgnoreCase));
            else
                chosenFeed = latestStreams.FirstOrDefault(f => f.Type.Equals("away", StringComparison.OrdinalIgnoreCase));

            if (chosenFeed == null)
            {
                chosenFeed = latestStreams.FirstOrDefault();
            }
            
            return chosenFeed;
        }

        public string GetStreamUrl(Feed feed)
        {
            var streamFetchUrl = $"https://nhl.freegamez.ga/getM3U8.php?league=nhl&date={feed.Date}&id={feed.MediaId}&cdn=akc";
            _messenger.WriteLine($"Trying to get stream URL from {streamFetchUrl}", Messenger.MessageCategory.Verbose);
            string streamUrl;

            using (var webClient = new System.Net.WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
                webClient.Headers.Add("accept", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                streamUrl = webClient.DownloadString(streamFetchUrl);
            }
            return streamUrl;
        }


        private RootObject GetModel(DateTime start, DateTime end)
        {
            string startDate, endDate;
            if (start == null)
                start = DateTime.Now.Subtract(new TimeSpan(48, 0, 0));
            if (end == null)
                end = DateTime.Now;

            startDate = start.ToString("yyyy-MM-dd");
            endDate = end.ToString("yyyy-MM-dd");

            string schedulerUrl = "https://statsapi.web.nhl.com/api/v1/schedule?startDate=" + startDate + "&endDate=" + endDate
                    + "&expand=schedule.teams,schedule.linescore,schedule.broadcasts.all,schedule.game.content.media.epg";
            _messenger.WriteLine($"Trying to get NHL schedule data from {schedulerUrl}", Messenger.MessageCategory.Verbose);
            RootObject model;

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(schedulerUrl);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                model = JsonSerializer.Deserialize<RootObject>(json, options);
            }
            return model;
        }

        private static List<Feed> GetFeeds(RootObject model)
        {
            var feeds = new List<Feed>();
            int idCounter = 1;

            foreach (var date in model.Dates)
            {
                foreach (var game in date.Games.Where(g => g.Status.DetailedState.Equals("Final", StringComparison.OrdinalIgnoreCase)))
                {
                    foreach (var epg in game.Content.Media.Epg.Where(e => e.Title.Equals("NHLTV", StringComparison.OrdinalIgnoreCase)))
                    {
                        foreach (var mediaFeed in epg.Items)
                        {
                            if (!mediaFeed.MediaFeedType.Equals("home", StringComparison.OrdinalIgnoreCase) && 
                                !mediaFeed.MediaFeedType.Equals("away", StringComparison.OrdinalIgnoreCase) && 
                                !mediaFeed.MediaFeedType.Equals("national", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            var feed = new Feed()
                            {
                                Home = game.Teams.Home.Team.Abbreviation,
                                Away = game.Teams.Away.Team.Abbreviation,
                                Date = date.Date,
                                Id = idCounter,
                                Name = mediaFeed.CallLetters,
                                MediaId = mediaFeed.MediaPlaybackId,
                                Type = mediaFeed.MediaFeedType.ToLower()
                            };
                            feeds.Add(feed);
                            idCounter++;
                        }
                    }                    
                }
            }
            return feeds;
        }
        
    }
}
