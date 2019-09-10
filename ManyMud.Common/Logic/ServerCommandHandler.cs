using ManyMud.Common.Interfaces;
using ManyMud.Common.ServerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManyMud.Common.Logic
{
    public class ServerCommandHandler
    {
        public IMessengerSet MessengerSet { get; }
        public ICommandSerializer Serializer { get; }
        public IFactory Factory { get; }

        public ServerCommandHandler(IFactory factory, IMessengerSet messengerSet, ICommandSerializer serializer)
        {
            Factory = factory;
            MessengerSet = messengerSet;
            Serializer = serializer;
            MessengerSet.ServerCommands.MessageReceived += ServerCommands_MessageReceived;
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
