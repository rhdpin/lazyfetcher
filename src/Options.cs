using CommandLine;

namespace LazyFetcher
{
    public class Options
    {
        [Option('c', "choose", Required = false, HelpText = "Choose the feed from list of found feeds.")]
        public bool Choose { get; set; }

        [Option('p', "path", Required = false, HelpText = "Set target download path.")]
        public string TargetPath { get; set; }

        [Option('t', "team", Required = false, HelpText = "Get latest game for team (three letter abbreviation. E.g. WPG).")]
        public string Team { get; set; }

        [Option('l', "league", Required = false, HelpText = "Set league (default: NHL).")]
        public string League { get; set; }

        [Option('u', "url", Required = false, HelpText = "Get only URL of the stream but don't download")]
        public bool OnlyUrl { get; set; }
    }
}
