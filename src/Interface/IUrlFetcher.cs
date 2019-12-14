using LazyFetcher.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyFetcher.Interface
{
    public interface IUrlFetcher
    {
        Feed GetLatestFeedForTeam(string teamName, DateTime startDate, DateTime endDate);
        string GetStreamUrl(Feed feed);
        IEnumerable<Feed> GetFeeds(DateTime startDate, DateTime endDate);
    }
}
