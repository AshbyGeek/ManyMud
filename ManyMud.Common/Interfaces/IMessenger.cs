using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Interfaces
{
    public interface IMessenger : IDisposable
    {
        event EventHandler<string> MessageReceived;

        void Send(string message);
    }
}
