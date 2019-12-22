using CommandLine;
using LazyFetcher.Interface;

namespace LazyFetcher
{
    public class Options : IOptions
    {
        [Option('c', "choose", SetName = "choose", Required = false, HelpText = "Choose the feed from list of found feeds.")]
        public bool Choose { get; set; }

        [Option('t', "team", SetName = "team", Required = false, HelpText = "Get latest game for team (three letter abbreviation. E.g. WPG).")]
        public string Team { get; set; }

        [Option('b', "bitrate", Required = false, HelpText = "Specify bitrate of stream to be downloaded (default: 'best'). Use verbose mode to see available bitrates.")]
        public string Bitrate { get; set; }

        [Option('l', "league", Required = false, HelpText = "Set league (default: NHL).")]
        public string League { get; set; }
        
        [Option('o', "overwrite", Required = false, HelpText = "Overwrite file if it already exists (default: download is skipped if file exists)")]
        public bool OverwriteExistingFile { get; set; }

        [Option('p', "path", Required = false, HelpText = "Set target download path.")]
        public string TargetPath { get; set; }

        [Option('u', "url", Required = false, HelpText = "Get only URL of the stream but don't download.")]
        public bool OnlyUrl { get; set; }
        
        [Option('x', "use-proxy", Required = false, HelpText = "Use proxy for redirection (required if 'hosts' file has not been edited).")]
        public bool UseProxy { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Use verbose mode to get more detailed output")]
        public bool VerboseMode { get; set; }
    }
}
