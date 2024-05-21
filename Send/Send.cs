using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            InputMessage();
        }

        static void InputMessage() {
            Console.WriteLine("Enter message to send:");
            string message = Console.ReadLine();

            SendMessage(message);
        }

        static void SendMessage(string message)
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

                    var body = Encoding.UTF8.GetBytes(message);

                    //Channel Basic
                    //channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);

                    //Channel Exchange
                    channel.BasicPublish(exchange: "test", routingKey: "", basicProperties: null, body: body);

                    Console.WriteLine($"Sent: {message}");
                }
            }
            InputMessage();
        }
    }
}
