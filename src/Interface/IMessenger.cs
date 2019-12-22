using LazyFetcher.Messenger;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyFetcher.Interface
{
    public interface IMessenger
    {
        void WriteLine(string line);

        void WriteLine(string line, MessageCategory category);
        
        void OverwriteLine(string line, MessageCategory category = MessageCategory.Normal);
    }
}
