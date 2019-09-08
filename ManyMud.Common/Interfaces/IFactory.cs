using ManyMud.Common.Interfaces;

namespace ManyMud.Common.Interfaces
{
    public interface IFactory
    {
        IGameResolver CreateGameResolver();

        IMessenger CreateMessenger(string hostname, int port, string messageBox);

        IMessenger CreateMessenger(GameHost host, string playerName);

        IPlayerMessengerSet CreatePlayerMessengerSet(GameHost host, string playerName);

        object CreateCommandHandler(IPlayerMessengerSet messengerSet, ICommandSerializer serializer);
    }
}