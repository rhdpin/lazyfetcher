
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

        string Bitrate { get; set; }

        bool OverwriteExistingFile { get; set; }

        bool VerboseMode { get; set; }

        public string Date { get; set; }

        public int Days { get; set; }
    }
}
