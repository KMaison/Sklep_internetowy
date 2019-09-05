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
        private static bool CallBuyingQueue(string message, IModel channel)
        {
            try
            {
                var service = new Service1();
                var index = message.IndexOf(";");
                var query1 = message.Substring(0, index);
                message = message.Substring(index + 1);
                ////query1
                Console.WriteLine(" [.] add client order ({0})", query1);             
                var response1 = service.CreateClientOrder(query1).ToString();
                Console.WriteLine("ClientOrder: " + response1);
                ///
                index = message.IndexOf(";");
                var query2 = message.Substring(0, index);
                message = message.Substring(index);
                //query2
                var comaIndex = query2.IndexOf(",");
                var name = query2.Substring(0, comaIndex);
                query2 = query2.Substring(comaIndex + 1);
                //comaIndex = query2.IndexOf(",");
                var surname = query2;
                var idorder = response1;
                Console.WriteLine(" [.] add client ({0})", name);

                //kolejka - AddClient (na nowym watku)
                var response2 = service.AddClient(name, surname, idorder).ToString();

                Console.WriteLine("AddClient: " + response2);
                //query3
                index = message.IndexOf(";");
                message = message.Substring(index + 1);
                while (!message.Equals(""))
                {

                    index = message.IndexOf(";");
                    var query3 = message.Substring(0, index);

                    message = message.Substring(index + 1);

                    comaIndex = query3.IndexOf(",");
                    var amount = query3.Substring(0, comaIndex);
                    query3 = query3.Substring(comaIndex + 1);
                    comaIndex = query3.IndexOf(",");
                    var barcode = "123457789";// query3;
                    var idorder1 = response1;

                    Console.WriteLine(" [.] add product order ({0})", barcode);
                    //kolejka - AddClient (na nowym watku)
                    var response3 = service.AddOrderProduct(amount, barcode, idorder1);
                    Console.WriteLine("OrderProduct: " + response3.ToString());
                    if (!response3)
                    {
                        throw new Exception();
                    }
                    //TODO: buy product                               
                }
            }
            catch
            {
                return false;
            }
            return true;

        }
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
                    else
                    {
                        var t = CallBuyingQueue(message, channel);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }


    }
}

