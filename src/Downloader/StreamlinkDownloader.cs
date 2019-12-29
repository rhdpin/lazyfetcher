using LazyFetcher.Data;
using LazyFetcher.Interface;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyFetcher.Downloader
{
    public class StreamlinkDownloader : IDownloader
    {
        private const string StreamLinkAppName = "streamlink";
        private Process _process = null;
        private readonly IMessenger _messenger;
        private readonly IOptions _options;
        private readonly IProxy _proxy;

        public StreamlinkDownloader(IMessenger messenger, IOptions options, IProxy proxy)
        {
            _messenger = messenger;
            _options = options;
            _proxy = proxy;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_process != null && !_process.HasExited)
                {
                    _process.Kill(true);
                }
            }
        }

        public void Download(DownloadRequest request)
        {
            // This dirty solution requires that StreamLink can be found from current dir or PATH
            var streamLinkAppName = "streamlink";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                streamLinkAppName += ".exe";
            }

            var targetDirectory = Path.GetDirectoryName(request.TargetFileName);
            if (request.TargetFileName != null && !targetDirectory.Equals(string.Empty) && !Directory.Exists(targetDirectory))
            {
                _messenger.WriteLine("Target path was not found. Do you want to create it? (y/n): ");
                var cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Y)
                {
                    Directory.CreateDirectory(targetDirectory);
                }
            }

            var proxyString = string.Empty;
            if (request.UseProxy)
            {                
                proxyString = $"--https-proxy https://127.0.0.1:{_proxy.Port}";
            }

            var loggingString = String.Empty;
            if (_options.VerboseMode)
            {
                loggingString = $"-v -l debug";
            }
            var streamUrl = request.StreamUrl.Replace("https://", "http://");                    
            
            var streamArgs = $"\"hlsvariant://{streamUrl} name_key=bitrate verify=False\" {_options.Bitrate} --http-header " +
                                $"\"User-Agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                                $"Chrome/59.0.3071.115 Safari/537.36\" --hls-segment-threads=4 {proxyString} {loggingString} -f -o {request.TargetFileName}";

            _messenger.WriteLine($"Starting download with command '{StreamLinkAppName} {streamArgs}", Messenger.MessageCategory.Verbose);
                        
            using (Process process = new Process())
            {
                process.StartInfo.FileName = streamLinkAppName;
                process.StartInfo.Arguments = streamArgs;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                StringBuilder output = new StringBuilder();                

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))                
                {
                    process.OutputDataReceived += (sender, e) => {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };                    

                    try
                    {
                        process.Start();
                    }
                    catch (Exception e)
                    {
                        _messenger.WriteLine($"Could not start downloader '{StreamLinkAppName}': {e.Message}. Ensure that the downloader application is located in current directory or $PATH.");
                        return;
                    }

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (!_options.VerboseMode)
                    {
                        Task.Run(() =>
                        {
                            var counter = 1;
                            long previousSize = 0;
                            DateTime previousTime = DateTime.Now;
                            double downloadRate = 0;
                            string downloadRateString;

                            while (process != null && !process.HasExited)
                            {
                                Thread.Sleep(500);

                                if (File.Exists(request.TargetFileName))
                                {
                                    var currentSize = new FileInfo(request.TargetFileName).Length;
                                    if (counter % 10 == 0)
                                    {
                                        var currentTime = DateTime.Now;
                                        var secondsElapsed = (currentTime - previousTime).TotalMilliseconds / (double)1000;
                                        var bytesDownloaded = currentSize - previousSize;
                                        downloadRate = (bytesDownloaded / 1024 / 1024 / secondsElapsed);
                                        previousSize = currentSize;
                                        previousTime = currentTime;
                                    }
                                    downloadRateString = (downloadRate > 0) ? downloadRate.ToString("0.0") : "---";

                                    _messenger.OverwriteLine($"Writing stream to file: {currentSize / 1024 / 1024} MB ({downloadRateString} MB/s)");
                                    counter++;
                                }
                            }
                        });
                       
                        _messenger.WriteLine("");

                        process.WaitForExit();

                        var unexpectedOutput = false;

                        // Write Streamlink output to console (other than the regular messages)                        
                        using (var stringReader = new StringReader(output.ToString()))
                        {
                            string line = null;
                            do
                            {
                                line = stringReader.ReadLine();

                                if (line != null)
                                {
                                    if (!line.StartsWith("[cli][info]") && !line.Contains("[download]"))
                                    {
                                        _messenger.WriteLine($"{line}");
                                        unexpectedOutput = true;
                                    }
                                    else if (_options.VerboseMode)
                                    {
                                        _messenger.WriteLine($"{line}");
                                    }
                                }
                            } while (line != null);
                        }                        

                        if (unexpectedOutput)
                        {
                            _messenger.WriteLine("\nLooks like something went wrong. Please check that redirection is configured either by editing " +
                                "hosts file or by using proxy (parameter '-x' and requires that mlbamproxy is found). " +
                                "By default application expects that hosts file is edited");
                        }
                    }
                    else
                    {
                        _messenger.WriteLine("Full output from Streamlink is displayed after process has exited. Please wait.", Messenger.MessageCategory.Verbose);

                        process.WaitForExit();

                        // Output all output from Streamlink after process has exited
                        _messenger.WriteLine(output.ToString());
                    }
                }
            }            
        }
    }
}
