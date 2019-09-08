using ManyMud.Common.ServerCommands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManyMud.Tests
{
    [TestClass]
    public class ServerCommandsTests
    {
        public static CommandSerializer Serializer = new CommandSerializer();

        [TestMethod]
        public void UpdatePlayersCommand_SerializationRoundTrip()
        {
            var cmd = new UpdatePlayersCommand();
            cmd.Players = new[] { "Test1", "Test2" };

            string json = Serializer.Serialize(cmd);

            Assert.IsFalse(string.IsNullOrWhiteSpace(json));
            Assert.IsTrue(json.Contains("Test1"));
            Assert.IsTrue(json.Contains("Test2"));

            if (Serializer.Deserialize(json) is UpdatePlayersCommand cmd2)
            {
                Assert.IsTrue(cmd.Players.Length == 2);
                Assert.AreEqual("Test1", cmd.Players[0]);
                Assert.AreEqual("Test2", cmd.Players[1]);
            }
            else
            {
                Assert.Fail();
            }
        }

        public void CommandSerializer_WontDeserializeFileSystemClass()
        {
            Assert.ThrowsException<Exception>(() => Serializer.Deserialize(Resources.RogueJson));
        }
    }
}
