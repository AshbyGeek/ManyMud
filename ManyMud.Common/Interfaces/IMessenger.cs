using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Common.Interfaces
{
    public interface IMessenger : IDisposable
    {
        string MessageBox { get; }

        event EventHandler<string> MessageReceived;

        void Send(string message);
    }
}
