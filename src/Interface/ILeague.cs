using LazyFetcher.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyFetcher.Interface
{
    public interface ILeague
    {
        string Name { get; }
        bool IsRedirectionRequired { get; }
        Type ProxyType { get; }
        LeagueType Type { get; }
    }
}
