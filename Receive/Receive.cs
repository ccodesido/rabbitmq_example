using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbit", Password = "rabbit_pwd" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //Channel Basic
                    //channel.QueueDeclare(queue: "hello", durable:false, exclusive:false, autoDelete:false, arguments:null);

                    //Channel Exchange
                    channel.ExchangeDeclare(exchange: "test", type: ExchangeType.Fanout);
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName, exchange: "test", routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine($"[x] Received {message}");
                    };

                    //Channel Basic
                    //channel.BasicConsume(queue: "hello", autoAck: true, consumer:consumer);

                    //Channel Exchange
                    channel.BasicConsume(queue:queueName, autoAck: true, consumer:consumer);
                    
                    Console.WriteLine("Awaiting messages...");
                    Console.ReadLine();
                }
            }
        }
    }
}