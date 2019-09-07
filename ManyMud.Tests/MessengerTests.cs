using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManyMud.Common.Models;
using System.Threading;

namespace ManyMud.Tests
{
    [TestClass]
    public class MessengerTests
    {
        public Messenger receiver1;
        public Messenger receiver2;
        public Messenger sender;

        const int MSG_FLIGHT_TIME = 50;

        [TestInitialize]
        public void TestSetup()
        {
            const string ServerHostName = "demo.portainer.io";
            const int ServerPort = 5672;
            receiver1 = new Messenger(ServerHostName, ServerPort, "Test");
            receiver2 = new Messenger(ServerHostName, ServerPort, "Test");
            sender = new Messenger(ServerHostName, ServerPort, "Test");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            receiver1.Dispose();
            sender.Dispose();
        }

        [TestMethod]
        public void Messenger_RoundtripWorks()
        {
            const string testMsg = "It's worse than that, he's DEAD Jim!";

            string receiverMsg = "";
            string senderMsg = "";
            receiver1.MessageReceived += (s, e) => receiverMsg = e;
            sender.MessageReceived += (s, e) => senderMsg = e;

            sender.Send(testMsg);

            // Got to give a little time for the message to pass to the server and come back again.
            Thread.Sleep(MSG_FLIGHT_TIME);

            Assert.AreEqual(testMsg, receiverMsg);
            Assert.AreEqual("", senderMsg);
        }

        [TestMethod]
        public void Messenger_MultipleReceivers()
        {
            const string testMsg = "It's worse than that, he's DEAD Jim!";

            string receiver1Msg = "";
            string receiver2Msg = "";
            receiver1.MessageReceived += (s, e) => receiver1Msg = e;
            receiver2.MessageReceived += (s, e) => receiver2Msg = e;

            sender.Send(testMsg);

            // Got to give a little time for the message to pass to the server and come back again.
            Thread.Sleep(MSG_FLIGHT_TIME);

            Assert.AreEqual(testMsg, receiver1Msg);
            Assert.AreEqual(testMsg, receiver2Msg);
        }

        [TestMethod]
        public void Messenger_IgnoresOwnMessage()
         {
            bool msgReceived = false;
            sender.MessageReceived += (s, e) => msgReceived = true;

            sender.Send("blah blah");

            // Got to give a little time for the message to pass to the server and come back again.
            Thread.Sleep(MSG_FLIGHT_TIME);

            Assert.IsFalse(msgReceived);
        }
    }
}
