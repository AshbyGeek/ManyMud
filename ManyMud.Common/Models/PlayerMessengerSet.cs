using ManyMud.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Common.Models
{
    public class PlayerMessengerSet
    {
        
        public PlayerMessengerSet(GameHost host, IMessenger serverBroadcast, IMessenger localPlayer)
        {
            Host = host;
            ServerBroadcast = serverBroadcast;
            LocalPlayer = localPlayer;
            OtherPlayers = new List<IMessenger>();
        }

        public readonly GameHost Host;
        public readonly IMessenger ServerBroadcast;
        public readonly IMessenger LocalPlayer;
        public readonly IList<IMessenger> OtherPlayers;
    }
}
