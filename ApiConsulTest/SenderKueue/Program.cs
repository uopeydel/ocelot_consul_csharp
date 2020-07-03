using RabbitMQ.Client;
using System;
using System.Text;

namespace SenderKueue
{

    class Program

    {

        private const string UserName = "guest";

        private const string Password = "guest";

        private const string HostName = "localhost";



        static void Main(string[] args)
        {

            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.UserName = ConnectionFactory.DefaultUser;
                connectionFactory.Password = ConnectionFactory.DefaultPass;
                connectionFactory.VirtualHost = "/";
                connectionFactory.HostName = "172.17.251.1292"; //IPv4 Address.
                connectionFactory.Port = AmqpTcpEndpoint.UseDefaultPort; // 15672;// AmqpTcpEndpoint.UseDefaultPort;
                                                                         //IConnection conn = connectionFactory.CreateConnection();

                //ConnectionFactory connectionFactory = new ConnectionFactory

                //{ 
                //    HostName = HostName, 
                //    UserName = UserName, 
                //    Password = Password, 
                //};

                var connection = connectionFactory.CreateConnection();

                //var channel = connection.CreateModel();
                IModel channel = connectionFactory
                .CreateConnection()
                .CreateModel();


                // accept only one unack-ed message at a time

                // uint prefetchSize, ushort prefetchCount, bool global



                channel.BasicQos(0, 1, false);

                MessageReceiver messageReceiver = new MessageReceiver(channel);

                channel.BasicConsume("testqueue", false, messageReceiver);
                
                Console.ReadLine();
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }


    }

    public class MessageReceiver : DefaultBasicConsumer

    {

        private readonly IModel _channel;

        public MessageReceiver(IModel channel)

        {

            _channel = channel;

        }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        //public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)

        {

            Console.WriteLine($"Consuming Message");

            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));

            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));

            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));

            Console.WriteLine(string.Concat("Routing tag: ", routingKey));

            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body.Span)));

            _channel.BasicAck(deliveryTag, false);

        }

    }
}
