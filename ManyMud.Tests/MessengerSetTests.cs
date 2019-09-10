using ManyMud.Common.Interfaces;
using ManyMud.Common.Models;
using ManyMud.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Tests
{
    [TestClass]
    public class MessengerSetTests
    {
        private static GameHost Host = new GameHost("localhost", 5672, "test");

        [TestMethod]
        public void PlayerMessengerSet_PlayerAddedFires()
        {

            var moqBroadcast = new Mock<IMessenger>();
            var moqCommand = new Mock<IMessenger>();
            var moqUser = new Mock<IMessenger>();
            var set = new MessengerSet(Host, moqCommand.Object, moqBroadcast.Object, moqUser.Object, "TestUser");

            var moqOtherUser = new Mock<IMessenger>();

            bool success = false;
            set.PlayerAdded += (s, e) => success = ReferenceEquals(e, moqOtherUser.Object);

            set.OtherPlayers.Add(moqOtherUser.Object);

            Assert.IsTrue(success);
        }


        [TestMethod]
        public void PlayerMessengerSet_PlayerRemovedFires()
        {
            var moqBroadcast = new Mock<IMessenger>();
            var moqCommand = new Mock<IMessenger>();
            var moqUser = new Mock<IMessenger>();
            var set = new MessengerSet(Host, moqCommand.Object, moqBroadcast.Object, moqUser.Object, "TestUser");

            var moqOtherUser = new Mock<IMessenger>();
            set.OtherPlayers.Add(moqOtherUser.Object);

            bool success = false;
            set.PlayerRemoved += (s, e) => success = ReferenceEquals(e, moqOtherUser.Object);

            set.OtherPlayers.Remove(moqOtherUser.Object);

            Assert.IsTrue(success);
        }
    }
}
