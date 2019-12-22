using CommandLine;
using System;
using System.Reflection;
using LazyFetcher.Interface;
using LazyFetcher.NHL;
using LightInject;
using LazyFetcher.Downloader;
using LazyFetcher.Data;
using LazyFetcher.Messenger;

namespace LazyFetcher
{
    class Program
    {        
        private static LeagueType _selectedLeague;        
        private static int _exitCode = 0;

        public static ServiceContainer IocContainer { get; private set; }

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (s, ev) =>
            {
                IocContainer?.Dispose();                
            };

            using (IocContainer = new ServiceContainer())
            {
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(options =>
                    {                        
                        if (options.League != null)
                        {
                            _selectedLeague = Enum.Parse<LeagueType>(options.League, true);
                        }
                        if (!options.OnlyUrl)
                        {
                            var version = Assembly.GetExecutingAssembly().GetName().Version;
                            Console.WriteLine($"{Assembly.GetExecutingAssembly().GetName().Name} {version.Major}.{version.Minor}.{version.Build}\n");                            
                        }                        
                    })
                    .WithNotParsed<Options>((errors) =>
                    {
                        _exitCode = -1;
                        return;
                    });

                if (_exitCode == -1)
                    return;                

                IocContainer.Register<IDownloader, StreamlinkDownloader>();

                switch (_selectedLeague)
                {
                    case LeagueType.NHL:
                        IocContainer.Register<ILeague, NHLLeague>();
                        IocContainer.Register<IProxy, NHLProxy>();
                        IocContainer.Register<IUrlFetcher, NHLUrlFetcher>();                        
                        break;
                }                                               

                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(options =>
                    {
                        IocContainer.Register<IOptions>(factory => options);
                        IocContainer.Register<IMessenger, ConsoleMessenger>();
                        IocContainer.Register<IFeedManager, DefaultFeedManager>();
                        
                        var feedManager = IocContainer.GetInstance<IFeedManager>();

                        if (options.Team != null)
                        {
                            feedManager.GetLatest(options.Team, options.TargetPath, options.OnlyUrl);
                        }
                        else if (options.Choose)
                        {
                            feedManager.ChooseFeed(options.TargetPath, options.OnlyUrl);
                        }
                        else
                        {
                            Console.WriteLine($"Help: {Assembly.GetExecutingAssembly().GetName().Name} --help");
                        }
                    });
            }
        }              
    }
}
