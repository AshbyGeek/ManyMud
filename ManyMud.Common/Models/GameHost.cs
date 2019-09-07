using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Common.Models
{
    public struct GameHost
    {
        public GameHost(string hostName, int port, string gameName)
        {
            Hostname = hostName;
            Port = port;
            GameName = gameName;
        }

        public readonly string Hostname;
        public readonly int Port;
        public readonly string GameName;
    }
}
