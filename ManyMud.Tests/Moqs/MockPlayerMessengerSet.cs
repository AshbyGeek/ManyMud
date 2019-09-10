using ManyMud.Common.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Tests.Mocks
{
    public class MockPlayerMessengerSet : Mock<IMessengerSet>
    {
        public GameHost Host { get; } = new GameHost("localhost", 5672, "Test");

        public string PlayerName { get; set; } = "TestUser";

        public Mock<IMessenger> Broadcast { get; set; } = new Mock<IMessenger>();

        public Mock<IMessenger> Command { get; set; } = new Mock<IMessenger>();

        public Mock<IMessenger> LocalUser { get; set; } = new Mock<IMessenger>();

        public List<IMessenger> OtherUsers { get; set; } = new List<IMessenger>();

        public MockPlayerMessengerSet()
        {
            Broadcast.SetupGet(x => x.MessageBox).Returns("Test.broadcast");
            Command.SetupGet(x => x.MessageBox).Returns("Test.command");
            LocalUser.SetupGet(x => x.MessageBox).Returns("Test.localUser");
            SetupGet(x => x.LocalPlayer).Returns(LocalUser.Object);
            SetupGet(x => x.ServerCommands).Returns(Command.Object);
            SetupGet(x => x.ServerBroadcast).Returns(Broadcast.Object);
            SetupGet(x => x.OtherPlayers).Returns(OtherUsers);
            SetupGet(x => x.Host).Returns(Host);
            SetupGet(x => x.LocalPlayerName).Returns(() => PlayerName);
        }
    }
}
