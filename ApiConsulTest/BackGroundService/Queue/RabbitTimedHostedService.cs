using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading;
using System.Threading.Tasks;

namespace BackGroundService.Queue
{
    public class RabbitTimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<RabbitTimedHostedService> _logger;
        private Timer _timer;

        public RabbitTimedHostedService(ILogger<RabbitTimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            RunRabbitQueue();
            return Task.CompletedTask;
        }
        private void DoWork(object state,IConnection rabbitConnection , IModel channel , string queueName)
        {

            var count = Interlocked.Increment(ref executionCount);
            if (executionCount == 14)
            {
                //Console.WriteLine("DISPOSED");
                //rabbitConnection.Dispose();
                //channel.Dispose();
                //Dispose();
            }

            string body = $"A nice random message: {DateTime.Now.Ticks}  Message sent { count} ";
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(body));

            Console.WriteLine(body);
        }


        private void RunRabbitQueue()
        {
            const string queueName = "testqueue2";

            try
            {

                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.UserName = ConnectionFactory.DefaultUser;
                connectionFactory.Password = ConnectionFactory.DefaultPass;
                connectionFactory.VirtualHost = "/";
                connectionFactory.HostName = "172.17.173.209"; //IPv4 Address.
                connectionFactory.Port = AmqpTcpEndpoint.UseDefaultPort;

                Console.WriteLine("CreateConnection");

                //IConnection conn = connectionFactory.CreateConnection();

                Console.WriteLine("connectionFactory CreateConnection");

                var rabbitConnection = connectionFactory.CreateConnection();

                Console.WriteLine("CreateModel");

                var channel = rabbitConnection.CreateModel();

                Console.WriteLine("QueueDeclare");

                channel.QueueDeclare(
                  queue: queueName,
                  durable: false,
                  exclusive: false,
                  autoDelete: false,
                  arguments: null);
                //_timer = new Timer((e) =>
                //{
                //    var count = Interlocked.Increment(ref executionCount);
                //    if (executionCount == 14)
                //    {
                //        Console.WriteLine("DISPOSED");
                //        rabbitConnection.Dispose();
                //        channel.Dispose();
                //        Dispose();
                //    }

                //    string body = $"A nice random message: {DateTime.Now.Ticks}  Message sent { count} ";
                //    channel.BasicPublish(
                //        exchange: string.Empty,
                //        routingKey: queueName,
                //        basicProperties: null,
                //        body: Encoding.UTF8.GetBytes(body));

                //    Console.WriteLine(body);
                //}, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                //while (true)
                //{
                //    var count = Interlocked.Increment(ref executionCount);
                //    if (executionCount == 14)
                //    {
                //        Console.WriteLine("DISPOSED");
                //        Dispose();
                //    }

                //    string body = $"A nice random message: {DateTime.Now.Ticks}  Message sent { count} ";
                //    channel.BasicPublish(
                //        exchange: string.Empty,
                //        routingKey: queueName,
                //        basicProperties: null,
                //        body: Encoding.UTF8.GetBytes(body));

                //    Console.WriteLine(body);
                //    Task.Delay(5000).Wait();
                //}
                _timer = new Timer((e) => DoWork(e, rabbitConnection , channel, queueName), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));



            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("End");
            Console.Read();

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
