using ManyMud.Common.Models;
using ManyMud.Interfaces;
using ManyMud.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Common.Logic
{
    public class MessengerManager
    {
        public MessengerManager()
        {
        }

        public static IEnumerable<GameHost> LocateGameHosts()
        {
            return new List<GameHost>() { new GameHost() { Hostname = "ashbygeek.ddns.net", Port = 5672, GameName = "Test" } };
        }

        //public static PlayerMessengerSet CreatePlayerMessengerSet(GameHost host)
        //{
        //    var set = new PlayerMessengerSet()
        //    {
        //        host = host,
        //        LocalPlayers = new Messenger()
        //    }
        //}
    }
}
