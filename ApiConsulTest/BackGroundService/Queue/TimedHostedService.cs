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
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            var data = "aaa";
            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.UserName = ConnectionFactory.DefaultUser;
                connectionFactory.Password = ConnectionFactory.DefaultPass;
                connectionFactory.VirtualHost = "/";
                connectionFactory.HostName = "172.17.53.225"; //IPv4 Address.
                connectionFactory.Port = AmqpTcpEndpoint.UseDefaultPort;
                IConnection conn = connectionFactory.CreateConnection();

                string queueName = "testqueue";
                var rabbitConnection = connectionFactory.CreateConnection();
                var channel = rabbitConnection.CreateModel();
                channel.QueueDeclare(
                  queue: queueName,
                  durable: false,
                  exclusive: false,
                  autoDelete: false,
                  arguments: null);
                _timer = new Timer((d) => DoWork2(d, rabbitConnection, channel, queueName), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                //_timer = new Timer((d) => DoWork(d, data), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            } catch (Exception e)
            { 
            
            }

            return Task.CompletedTask;
        }

        private void DoWork(object  da , string data)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }
        private void DoWork2(object state, IConnection rabbitConnection, IModel channel, string queueName)
        {

            var count = Interlocked.Increment(ref executionCount);
           

            string body = $"A nice random message: {DateTime.Now.Ticks}  Message sent { count} ";
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(body));

            Console.WriteLine(body);
            if (executionCount == 4)
            {
                Console.WriteLine("DISPOSED");
                rabbitConnection.Dispose();
                channel.Dispose();
                Dispose();
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("StopAsync");
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");
            _timer?.Dispose();
        }
    }
}
