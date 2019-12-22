using LazyFetcher.Interface;
using System;

namespace LazyFetcher.Messenger
{
    public class ConsoleMessenger : IMessenger
    {
        private readonly IOptions _options;

        public ConsoleMessenger(IOptions options)
        {
            _options = options;
        }

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public void WriteLine(string line, MessageCategory category)
        {
            if (category == MessageCategory.Verbose && !_options.VerboseMode)
            {
                return;
            }

            WriteLine(line);
        }

        public void OverwriteLine(string line, MessageCategory category = MessageCategory.Normal)
        {
            if (category == MessageCategory.Verbose && !_options.VerboseMode)
            {
                return;
            }

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(line);
        }
    }
}
