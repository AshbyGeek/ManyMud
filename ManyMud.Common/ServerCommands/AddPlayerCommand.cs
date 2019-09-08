using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Common.ServerCommands
{
    /// <summary>
    /// Sent by a client to add this user to the server. Server will respond with an UpdatePlayersCommand.
    /// </summary>
    public class AddPlayerCommand
    {
        public string PlayerName;
    }
}
