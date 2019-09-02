using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManyMud.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ManyMud.Models
{
    public class Messenger : IMessenger
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;

        private readonly string _queueName;
        private readonly Queue<byte[]> _ignoreMessages = new Queue<byte[]>();
        private readonly object _ignoreMessageLock = new object();

        public Messenger(string hostName, int port, string msgBoxName)
        {
            _queueName = msgBoxName;

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Create an exchange that passes new messages to all connected queues
            _channel.ExchangeDeclare(msgBoxName, "fanout");

            // Create a queue with a random name and tell the exchange to pass messages to it
            var tmpName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(tmpName, msgBoxName, "");

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, e) =>
            {
                lock (_ignoreMessageLock)
                {
                    if (_ignoreMessages.Count > 0 && e.Body.SequenceEqual(_ignoreMessages.Peek()))
                    {
                        _ignoreMessages.Dequeue();
                    }
                    else
                    {
                        MessageReceived?.Invoke(this, Decode(e.Body));
                    }
                }
            };

            _channel.BasicConsume(queue: tmpName,
                                  autoAck: true,
                                  consumer: _consumer);
        }


        public event EventHandler<string> MessageReceived;


        public void Send(string message)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(Messenger));

            var msg = Encode(message);
            _channel.BasicPublish(exchange: _queueName,
                                  routingKey: "",
                                  basicProperties: null,
                                  body: msg);
            lock (_ignoreMessageLock)
            {
                _ignoreMessages.Enqueue(msg);
            }
        }

        private byte[] Encode(string message) => Encoding.UTF8.GetBytes(message);

        private string Decode(byte[] message) => Encoding.UTF8.GetString(message);



        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _channel.Dispose();
                _connection.Dispose();
                _disposed = true;
            }
        }
        private bool _disposed = false; // To detect redundant calls
    }
}
