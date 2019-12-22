using System;
using System.Collections.Generic;
using System.Text;

namespace LazyFetcher.Interface
{
    public interface IFeedManager
    {
        void ChooseFeed(string targetPath, bool getOnlyUrl);

        public void GetLatest(string teamName, string targetPath, bool getOnlyUrl);
    }
}
