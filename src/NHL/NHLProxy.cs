using LazyFetcher.Interface;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace LazyFetcher.NHL
{
    public class NHLProxy : IProxy
    {
        private Process _process = null;
        private readonly IMessenger _messenger;

        public string ExecutablePath { get; }
        public string DestinationDomain => "freegamez.ga";
        public string SourceDomains => "mf.svc.nhl.com";
        public int Port => 19073;       

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
                    _process.Dispose();
                }
            }
        }

        public NHLProxy(IMessenger messenger)
        {
            _messenger = messenger;
            var proxyAppNameFileExtension = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                proxyAppNameFileExtension += ".exe";
            }

            // Fixed path means that executable must be found in PATH or in current directory
            ExecutablePath = $"mlbamproxy{proxyAppNameFileExtension}";
        }

        public bool Start()
        {
            var args = $"-p {Port} -d {DestinationDomain} -s {SourceDomains}";

            _messenger.WriteLine($"Starting proxy with command '{ExecutablePath} {args}'", Messenger.MessageCategory.Verbose);
            _process = new Process() { StartInfo = new ProcessStartInfo(ExecutablePath, args) { UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = false }, EnableRaisingEvents = true };

            try
            {
                _process.Start();
            }
            catch (Exception e)
            {
                _messenger.WriteLine($"Could not start proxy '{ExecutablePath}': {e.Message}. Ensure that the proxy application is located in current directory or $PATH.");
                return false;
            }

            while (!_process.StandardOutput.EndOfStream)
            {
                var line = _process.StandardOutput.ReadLine();
                if (line.Contains("Proxy server listening on port", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Thread.Sleep(50);
            }
            return true;
        }

        public bool Stop()
        {
            try
            {
                _process?.Kill(true);
            }
            catch (InvalidOperationException)
            {
                // Process has exited before
            }
            return true;
        }
    }
}
