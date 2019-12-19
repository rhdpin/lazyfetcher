using System;
using System.Collections.Generic;
using System.Text;

namespace LazyFetcher.Interface
{
    public interface IOptions
    {
        bool Choose { get; set; }
                
        string TargetPath { get; set; }

        string Team { get; set; }

        string League { get; set; }

        bool OnlyUrl { get; set; }

        bool UseProxy { get; set; }
    }
}
