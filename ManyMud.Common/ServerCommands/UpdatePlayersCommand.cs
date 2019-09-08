using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Common.ServerCommands
{
    /// <summary>
    /// Sent by the server to command each player's GUI to update for the new player list.
    /// </summary>
    public class UpdatePlayersCommand
    {
        /// <summary>
        /// List of all Players currently in the session.
        /// </summary>
        public string[] Players;
    }
}
