﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "hello", Password = "world" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello", durable:false, exclusive:false, autoDelete:false, arguments:null);
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine($"[x] Received {message}");
                    };

                    channel.BasicConsume(queue: "hello", autoAck: true, consumer:consumer);
                    
                    Console.WriteLine("Awaiting messages...");
                    Console.ReadLine();
                }
            }
        }
    }
}