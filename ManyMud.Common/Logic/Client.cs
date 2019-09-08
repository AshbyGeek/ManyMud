using ManyMud.Common.Interfaces;
using ManyMud.Common.ServerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManyMud.Common.Logic
{
    public class Client
    {
        public IFactory Factory;
        public IPlayerMessengerSet MessengerSet;
        public ICommandSerializer Serializer;

        public Client(GameHost host, string playerName, IFactory factory, ICommandSerializer serializer)
        {
            Factory = factory;
            Serializer = serializer;
            MessengerSet = Factory.CreatePlayerMessengerSet(host, playerName);
            MessengerSet.ServerCommands.MessageReceived += ServerCommands_MessageReceived;

            var cmd = new AddPlayerCommand()
            {
                PlayerName = playerName
            };
            MessengerSet.ServerCommands.Send(Serializer.Serialize(cmd));
        }

        private void ServerCommands_MessageReceived(object sender, string e)
        {
            var obj = Serializer.Deserialize(e);
            if (obj is UpdatePlayersCommand cmd)
            {
                IEnumerable<string> otherPlayerNames = MessengerSet.OtherPlayers.Select(x => x.MessageBox).Select(x => x.Split('.').Last());
                foreach (string newPlayer in cmd.Players.Except(otherPlayerNames))
                {
                    if (newPlayer != MessengerSet.LocalPlayerName)
                    {
                        var messenger = Factory.CreateMessenger(MessengerSet.Host, newPlayer);
                        MessengerSet.OtherPlayers.Add(messenger);
                    }
                }

                foreach (string oldPlayer in otherPlayerNames.Except(cmd.Players).ToArray()) // need to enumerate results so we can modify the collection in the loop
                {
                    var messenger = MessengerSet.OtherPlayers.First(x => x.MessageBox.EndsWith("." + oldPlayer));
                    MessengerSet.OtherPlayers.Remove(messenger);
                }
            }
        }
    }
}
