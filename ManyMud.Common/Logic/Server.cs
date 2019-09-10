using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ManyMud.Common.Interfaces;
using ManyMud.Common.ServerCommands;

namespace ManyMud.Common.Logic
{
    public class Server
    {
        public IFactory Factory;
        public IMessengerSet MessengerSet;
        public readonly Dictionary<string, IMessenger> Players = new Dictionary<string, IMessenger>();

        public Server(GameHost host, IFactory factory, ICommandSerializer serializer)
        {
            Factory = factory;
            Serializer = serializer;
            MessengerSet = Factory.CreatePlayerMessengerSet(host, "");

            MessengerSet.ServerCommands.MessageReceived += ServerCommands_MessageReceived;
        }

        public ICommandSerializer Serializer { get; }

        private void ServerCommands_MessageReceived(object sender, string e)
        {
            var obj = Serializer.Deserialize(e);
            if (obj is AddPlayerCommand cmd)
            {
                if (!Players.ContainsKey(cmd.PlayerName))
                {
                    var messenger = Factory.CreateMessenger(MessengerSet.Host, cmd.PlayerName);
                    Players.Add(cmd.PlayerName, messenger);
                    MessengerSet.OtherPlayers.Add(messenger);

                    MessengerSet.ServerBroadcast.Send("Welcome " + cmd.PlayerName + "!"); // TODO: make this welcome message more awesome somehow.
                }

                // Let everyone know the current state of users
                var updateCmd = new UpdatePlayersCommand() { Players = Players.Keys.ToArray() };
                var msg = Serializer.Serialize(updateCmd);
                MessengerSet.ServerCommands.Send(msg);
            }
        }
    }
}
