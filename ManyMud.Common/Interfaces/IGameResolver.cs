using System.Collections.Generic;
using ManyMud.Common.Interfaces;

namespace ManyMud.Common.Interfaces
{
    public interface IGameResolver
    {
        IEnumerable<GameHost> LocateGameHosts();
    }
}