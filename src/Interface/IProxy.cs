using System;
using System.Collections.Generic;
using System.Text;

namespace LazyFetcher.Interface
{
    public interface IProxy : IDisposable
    {
        string ExecutablePath { get; }
        string DestinationDomain { get; }
        string SourceDomains { get; }
        int Port { get;  }        

        bool Start();
        bool Stop();
    }
}
