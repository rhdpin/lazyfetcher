using LazyFetcher.Data;
using LazyFetcher.Interface;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LazyFetcher.Downloader
{
    public class StreamlinkDownloader : IDownloader
    {
        private const string StreamLinkAppName = "streamlink";
        private Process _process = null;
        
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
                Console.Write("Target path was not found. Do you want to create it? (y/n): ");
                var cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Y)
                {
                    Directory.CreateDirectory(targetDirectory);
                }
            }

            var proxyString = string.Empty;
            if (request.IsProxyRequired)
            {
                proxyString = $"--https-proxy https://127.0.0.1:{request.ProxyPort}";
            }

            var streamUrl = request.StreamUrl.Replace("https://", "http://");

            // Uses fixed stream quality (="best")
            var streamArgs = $"\"hlsvariant://{streamUrl} name_key=bitrate verify=False\" best --http-header " +
                                $"\"User-Agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                                $"Chrome/59.0.3071.115 Safari/537.36\" --hls-segment-threads=4 {proxyString} -o {request.TargetFileName}";

            _process = new Process()
            {
                StartInfo = new ProcessStartInfo(streamLinkAppName, streamArgs)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false                     
                }
            };
                        
            try
            {
                _process.Start();
            }
            catch (Exception e)
            {                
                Console.WriteLine($"Could not start downloader '{StreamLinkAppName}': {e.Message}. Ensure that the downloader application is located in current directory or $PATH.");
                return;
            }            
                        
            Task.Run(() =>
            {
                var counter = 1;
                long previousSize = 0;                                
                DateTime previousTime = DateTime.Now;
                double downloadRate = 0;
                string downloadRateString;

                while (!_process.HasExited)
                {
                    Thread.Sleep(500);
                    
                    if (File.Exists(request.TargetFileName))
                    {
                        var currentSize = new FileInfo(request.TargetFileName).Length;
                        if (counter % 10 == 0)
                        {                            
                            var currentTime = DateTime.Now;                            
                            var secondsElapsed = (currentTime - previousTime).TotalMilliseconds / (double) 1000;
                            var bytesDownloaded = currentSize - previousSize;
                            downloadRate = (bytesDownloaded / 1024 / 1024 / secondsElapsed);                            
                            previousSize = currentSize;
                            previousTime = currentTime;
                        }
                        downloadRateString = (downloadRate > 0) ? downloadRate.ToString("0.0") : "---";                        

                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write($"Writing stream to file: {currentSize / 1024 / 1024} MB ({downloadRateString} MB/s)");
                        counter++;
                    }
                }
            });

            _process.WaitForExit();
            Console.WriteLine();

            // Write Streamlink output to console (other than the regular messages)
            while (!_process.StandardOutput.EndOfStream)
            {
                var line = _process.StandardOutput.ReadLine();

                if (line != null && !line.StartsWith("[cli][info]") && !line.Contains("[download]"))
                {
                    Console.WriteLine($"{line}");                    
                }
            }            
        }
    }
}
