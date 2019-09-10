using ManyMud.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ManyMud.Common.Models
{
    public class MessengerSet : IMessengerSet
    {
        public event EventHandler<IMessenger> PlayerAdded;
        public event EventHandler<IMessenger> PlayerRemoved;

        public string LocalPlayerName { get; }

        public MessengerSet(GameHost host, IMessenger serverCommands, IMessenger serverBroadcast, IMessenger localPlayer, string localPlayerName)
        {
            Host = host;
            ServerCommands = serverCommands;
            ServerBroadcast = serverBroadcast;
            LocalPlayer = localPlayer;
            LocalPlayerName = localPlayerName;

            var tmp = new ObservableCollection<IMessenger>();
            tmp.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (IMessenger item in e.NewItems)
                    {
                        PlayerAdded?.Invoke(this, item);
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (IMessenger item in e.OldItems)
                    {
                        PlayerRemoved?.Invoke(this, item);
                    }
                }
            };
            OtherPlayers = tmp;
        }

        public GameHost Host { get; }
        public IMessenger ServerBroadcast { get; }
        public IMessenger ServerCommands { get; }
        public IMessenger LocalPlayer { get; }
        public IList<IMessenger> OtherPlayers { get; }
    }
}
