using ManyMud.Common.Interfaces;
using ManyMud.Common.Logic;
using ManyMud.Common.ServerCommands;
using ManyMud.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManyMud.Tests
{
    [TestClass]
    public class ClientTests
    {
        public MockMudFactory factory;
        public MockPlayerMessengerSet set;
        public Mock<ICommandSerializer> serializer;
        public Client client;

        [TestInitialize]
        public void Initialize()
        {
            factory = new MockMudFactory();
            serializer = new Mock<ICommandSerializer>();
            client = new Client(new GameHost("localHost", 5672, "Test"), "Test1", factory.Object, serializer.Object);
            set = Mock.Get(client.MessengerSet) as MockPlayerMessengerSet;
        }

        [TestMethod]
        public void Client_AddsNewPlayer()
        {
            var user1 = new Mock<IMessenger>();
            user1.SetupGet(x => x.MessageBox).Returns("Test.Test1");
            set.OtherUsers.Add(user1.Object);

            var user3 = new Mock<IMessenger>();
            user3.SetupGet(x => x.MessageBox).Returns("Test.Test3");
            set.OtherUsers.Add(user3.Object);

            factory.MoqMessenger.Setup(x => x.MessageBox).Returns("Test.Test2");

            var serverCmd = new UpdatePlayersCommand()
            {
                Players = new[] { "Test1", "Test2", "Test3" }
            };
            serializer.Setup(x => x.Deserialize(It.IsAny<string>())).Returns(serverCmd);

            // Raising this command should cause the serializer to deserialize "BogusJson"
            // 'serverCmd.Players' will then be compared against set.OtherUsers and the factory
            // will be used to create a new messenger for the new user.
            set.Command.Raise(x => x.MessageReceived += null, null, "BogusJson");

            // Make sure that we managed to get all the way from event raising to the new messenger in the set
            Assert.IsTrue(set.OtherUsers.Contains(factory.MoqMessenger.Object));
        }

        [TestMethod]
        public void Client_RemovesPlayer()
        {
            var user1 = new Mock<IMessenger>();
            user1.SetupGet(x => x.MessageBox).Returns("Test.Test1");
            set.OtherUsers.Add(user1.Object);

            var user2 = new Mock<IMessenger>();
            user2.SetupGet(x => x.MessageBox).Returns("Test.Test2");
            set.OtherUsers.Add(user2.Object);

            var user3 = new Mock<IMessenger>();
            user3.SetupGet(x => x.MessageBox).Returns("Test.Test3");
            set.OtherUsers.Add(user3.Object);

            var serverCmd = new UpdatePlayersCommand()
            {
                Players = new[] { "Test1", "Test3" }
            };
            serializer.Setup(x => x.Deserialize(It.IsAny<string>())).Returns(serverCmd);

            // Raising this command should cause the serializer to deserialize "BogusJson"
            // 'serverCmd.Players' will then be compared against set.OtherUsers and removals will
            // be made as appropriate.
            set.Command.Raise(x => x.MessageReceived += null, null, "BogusJson");

            // Make sure that we managed to get all the way from event raising to the new messenger in the set
            Assert.IsFalse(set.OtherUsers.Contains(user2.Object));
        }

        [TestMethod]
        public void Client_DoesntAddLocalUser()
        {
            var user1 = new Mock<IMessenger>();
            user1.SetupGet(x => x.MessageBox).Returns("Test.Test1");
            set.OtherUsers.Add(user1.Object);

            var user3 = new Mock<IMessenger>();
            user3.SetupGet(x => x.MessageBox).Returns("Test.Test3");
            set.OtherUsers.Add(user3.Object);

            set.LocalUser.SetupGet(x => x.MessageBox).Returns("Test.Test2");

            factory.MoqMessenger.Setup(x => x.MessageBox).Returns("Test.Test2");

            var serverCmd = new UpdatePlayersCommand()
            {
                Players = new[] { "Test1", "Test2", "Test3" }
            };
            serializer.Setup(x => x.Deserialize(It.IsAny<string>())).Returns(serverCmd);

            // Raising this command should cause the serializer to deserialize "BogusJson"
            // 'serverCmd.Players' will then be compared against set.OtherUsers and the factory
            // will be used to create a new messenger for the new users.
            // In this case, there are no new users because Test2 is the local user
            set.Command.Raise(x => x.MessageReceived += null, null, "BogusJson");

            Assert.IsFalse(set.OtherUsers.Contains(set.LocalUser.Object));
        }
    }
}
