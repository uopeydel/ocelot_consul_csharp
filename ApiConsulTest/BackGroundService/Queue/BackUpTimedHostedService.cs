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
    public class BackUpTimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<RabbitTimedHostedService> _logger;
        private Timer _timer;

        public BackUpTimedHostedService(ILogger<RabbitTimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            //_timer = new Timer(
            //    DoWork, null, 
            //    TimeSpan.Zero,
            //    TimeSpan.FromSeconds(5));

            var timer = new System.Threading.Timer(async (e) =>
            {
                await RunRabbitQueue(e);
                Console.WriteLine("Tick");
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
             

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            if (executionCount == 14)
            {
                Dispose();
            }
            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }

        private async Task RunRabbitQueue(object state)
        {
            //var count = Interlocked.Increment(ref executionCount);
            //if (executionCount == 14)
            //{
            //    Dispose();
            //}

            const string queueName = "testqueue";

            try
            {
                
                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.UserName = ConnectionFactory.DefaultUser;
                connectionFactory.Password = ConnectionFactory.DefaultPass;
                connectionFactory.VirtualHost = "/"; 
                connectionFactory.HostName = "172.17.53.225"; //IPv4 Address.
                connectionFactory.Port = AmqpTcpEndpoint.UseDefaultPort;
                IConnection conn = connectionFactory.CreateConnection();


                using (var rabbitConnection = connectionFactory.CreateConnection())
                {
                    using (var channel = rabbitConnection.CreateModel())
                    { 
                        channel.QueueDeclare(
                            queue: queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        while (true)
                        {
                            string body = $"A nice random message: {DateTime.Now.Ticks}";
                            channel.BasicPublish(
                                exchange: string.Empty,
                                routingKey: queueName,
                                basicProperties: null,
                                body: Encoding.UTF8.GetBytes(body));

                            Console.WriteLine("Message sent");
                            await Task.Delay(500);
                        }
                    }
                }
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
