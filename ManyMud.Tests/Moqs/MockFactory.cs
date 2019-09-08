using ManyMud.Common.Interfaces;
using ManyMud.Tests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManyMud.Tests.Mocks
{

    public class MockMudFactory : Mock<IFactory>
    {
        public List<GameHost> GameHosts { get; } = new List<GameHost>() { new GameHost("ashbygeek.ddns.net", 5672, "Test") };
        public Mock<IGameResolver> MoqGameResolver { get; set; }
        public Mock<IMessenger> MoqMessenger { get; set; }
        public MockPlayerMessengerSet MockPlayerMessengerSet { get; set; }
        public Mock<ICommandSerializer> MoqSerializer { get; set; }

        public MockMudFactory()
        {
            MoqGameResolver = new Mock<IGameResolver>();
            MoqGameResolver.Setup(x => x.LocateGameHosts()).Returns(GameHosts);
            Setup(x => x.CreateGameResolver()).Returns(MoqGameResolver.Object);

            MoqMessenger = new Mock<IMessenger>();
            MoqMessenger.SetupGet(x => x.MessageBox).Returns("Test.Test1");
            Setup(x => x.CreateMessenger(It.IsAny<GameHost>(), It.IsAny<string>())).Returns(MoqMessenger.Object);
            Setup(x => x.CreateMessenger(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Returns(MoqMessenger.Object);

            MoqSerializer = new Mock<ICommandSerializer>();

            MockPlayerMessengerSet = new MockPlayerMessengerSet();
            Setup(x => x.CreatePlayerMessengerSet(It.IsAny<GameHost>(), It.IsAny<string>())).Returns(MockPlayerMessengerSet.Object);


        }
    }
}
