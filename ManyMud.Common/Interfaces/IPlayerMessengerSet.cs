using System;
using System.Collections.Generic;
using ManyMud.Common.Interfaces;

namespace ManyMud.Common.Interfaces
{
    public interface IPlayerMessengerSet
    {
        GameHost Host { get; }
        string LocalPlayerName { get; }

        IMessenger ServerBroadcast { get; }
        IMessenger ServerCommands { get; }

        IMessenger LocalPlayer { get; }

        IList<IMessenger> OtherPlayers { get; }

        event EventHandler<IMessenger> PlayerAdded;
        event EventHandler<IMessenger> PlayerRemoved;
    }
}