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

        [Option('b', "bitrate", Required = false, Default = "best", HelpText = "Specify bitrate of stream to be downloaded. Use verbose mode to see available bitrates.")]
        public string Bitrate { get; set; }

        [Option('d', "days", Required = false, Default = 2, HelpText = "Specify how many days back to search games")]
        public int Days { get; set; }

        [Option('e', "date", Required = false, HelpText = "(Default: current date) Specify date to search games from (in format yyyy-MM-dd, e.g. 2019-12-22")]
        public string Date { get; set; }

        [Option('l', "league", Required = false, Default = "NHL", HelpText = "Set league. Only NHL supported currently.")]
        public string League { get; set; }
        
        [Option('o', "overwrite", Required = false, HelpText = "Overwrite file if it already exists (instead of skipping the download).")]
        public bool OverwriteExistingFile { get; set; }

        [Option('p', "path", Required = false, HelpText = "(Default: current directory) Set target download path.")]
        public string TargetPath { get; set; }

        [Option('u', "url", Required = false, HelpText = "Get only URL of the stream but don't download.")]
        public bool OnlyUrl { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Use verbose mode to get more detailed output.")]
        public bool VerboseMode { get; set; }

        [Option('x', "use-proxy", Required = false, HelpText = "Use proxy for redirection (required if 'hosts' file has not been edited).")]
        public bool UseProxy { get; set; }        
    }
}
