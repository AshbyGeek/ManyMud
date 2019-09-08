using ManyMud.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Common.Logic
{
    public class GameResolver : IGameResolver
    {
        public IEnumerable<GameHost> LocateGameHosts() => new List<GameHost>() { new GameHost(hostName: "ashbygeek.ddns.net", port: 5672, gameName: "Test") };
    }
}
