using System;
using System.Collections.Generic;
using System.Text;
using LazyFetcher.Data;
using LazyFetcher.Interface;

namespace LazyFetcher.NHL
{
    public class NHLLeague : ILeague
    {
        public string Name => "NHL";
        public bool IsRedirectionRequired => true;
        public Type ProxyType => typeof(NHLProxy);
        public LeagueType Type => LeagueType.NHL;
    }
}
