using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFServiceWebRole1;
namespace Rabbit
{
    class Program
    {
        public static void Main()
        {
            var service = new Service1();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "reservation_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "buying_queue", durable: false,
                 exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);

                channel.BasicConsume(queue: "reservation_queue",
                  autoAck: false, consumer: consumer);
                channel.BasicConsume(queue: "buying_queue",
                  autoAck: false, consumer: consumer);

                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;
                    //
                    var message = Encoding.UTF8.GetString(body);
                    var index = message.IndexOf("?");
                    var functionName = message.Substring(0, index);
                    message = message.Substring(index + 1);
                    if (functionName.Equals("ReserveProduct"))
                    {
                        try
                        {
                            var comaIndex = message.IndexOf(",");
                            var key = message.Substring(0, comaIndex);
                            var amount = message.Substring(comaIndex + 1);

                            Console.WriteLine(" [.] reserve ({0})", message);

                            response = service.ReserveProduct(key, amount).ToString();

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" [.] " + e.Message);
                            response = "";
                        }
                        finally
                        {
                            var responseBytes = Encoding.UTF8.GetBytes(response);
                            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                              basicProperties: replyProps, body: responseBytes);
                            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                              multiple: false);
                        }
                    }
                   
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        
        
    }
}

