using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using WCFServiceWebRole1;

namespace RabbitMQ
{
    public class Program
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
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "reservation_queue",
                  autoAck: false, consumer: consumer);
                channel.QueueDeclare(queue: "buying_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
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
                    if (ea.RoutingKey == "reservation_queue")
                    {
                        try
                        {
                            var message = Encoding.UTF8.GetString(body);
                            var comaIndex = message.IndexOf(",");
                            var key = message.Substring(0, comaIndex);
                            var amount = message.Substring(comaIndex + 1);


                            Console.WriteLine(" [.] reserveProduct({0},{1})", key, amount);

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
                    else if(ea.RoutingKey == "buying_queue")
                    {
                        try
                        {
                            var message = Encoding.UTF8.GetString(body);

                            var index = message.IndexOf("?");
                            var functionName = message.Substring(0, index);
                            message = message.Substring(index+1);

                            if (functionName == "ClientOrder")
                            {
                                Console.WriteLine(" [.] add client order({0})", message);
                                response = service.CreateClientOrder(message).ToString();
                            }
                            else if (functionName == "Client")
                            {
                                Console.WriteLine(" [.] add client ({0})", message);
                                index = message.IndexOf(",");
                                var name = message.Substring(0, index);
                                message = message.Substring(index+1);
                                index = message.IndexOf(",");
                                var surname = message.Substring(0, index);
                                var order_id = message.Substring(index+1);
                                response = service.AddClient(name,surname,order_id).ToString();
                            }
                            else if (functionName == "OrderProduct")
                            {
                                Console.WriteLine(" [.] add order product ({0})", message);
                                index = message.IndexOf(",");
                                var amount = message.Substring(0, index);
                                message = message.Substring(index + 1);
                                index = message.IndexOf(",");
                                var bar_code = message.Substring(0, index);
                                var id_client_order = message.Substring(index + 1);
                                response = service.AddOrderProduct(amount,  bar_code,  id_client_order).ToString();
                            }

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