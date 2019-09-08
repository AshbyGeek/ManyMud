using ManyMud.Common.Models;
using ManyMud.Common.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ManyMud.Common.ServerCommands;

namespace ManyMud.Common.Logic
{
    public class Factory : IFactory
    {
        public IGameResolver CreateGameResolver() => new GameResolver();

        public IPlayerMessengerSet CreatePlayerMessengerSet(GameHost host, string playerName)
        {
            IMessenger serverBroadcastMsgr = CreateMessenger(host, "broadcast");
            IMessenger serverCommandsMsgr = CreateMessenger(host, "command");
            IMessenger localPlayerMsgr = CreateMessenger(host, playerName);
            return new PlayerMessengerSet(host, serverCommandsMsgr, serverBroadcastMsgr, localPlayerMsgr, playerName);
        }

        public IMessenger CreateMessenger(GameHost host, string playerName)
        {
            return new Messenger(host.Hostname, host.Port, MsgBoxName(host.GameName, playerName));
        }

        public IMessenger CreateMessenger(string hostname, int port, string messageBox)
        {
            return new Messenger(hostname, port, messageBox);
        }

        public object CreateCommandHandler(IPlayerMessengerSet messengerSet, ICommandSerializer serializer) => new ServerCommandHandler(this, messengerSet, serializer);

        private static string MsgBoxName(string gameName, string playerName) => $"{gameName}.{playerName}";
    }
}
