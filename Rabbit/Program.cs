using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFServiceWebRole1;
using System.Threading;
using System.Diagnostics;

namespace Rabbit
{
    class OrderThreadClass
    {
        protected Service1 service; 
        protected string order_id;

        public OrderThreadClass()
        {
            service = new Service1();
        }

        public void CreateClientOrder(object data)
        {
            Console.WriteLine(" [.] add client order ({0})", data.ToString());
            order_id =service.CreateClientOrder(data.ToString()).ToString();
            Console.WriteLine("ClientOrder: " + order_id);

        }
        public void AddClient(object data)
        {
            string[] parameters = data.ToString().Split(',');

            Console.WriteLine(" [.] add client ({0})", parameters[0]);
            
            var response2 = service.AddClient(parameters[0], parameters[1], order_id).ToString();

            Console.WriteLine("AddClient: " + response2);

        }
        public void AddOrderProduct(object data)
        {
            if (data.ToString().Equals("")) return;
            string[] parameters = data.ToString().Split(',');
            Console.WriteLine(" [.] add product order ({0})", parameters[1]);
            var response3 = service.AddOrderProduct(parameters[0], parameters[1], order_id);
            Console.WriteLine("OrderProduct: " + response3.ToString());


        }
    }
    class Program
    {
        private static bool CallBuyingQueue(string message, IModel channel)
        {
            try
            {
                OrderThreadClass oMyThreadClass = new OrderThreadClass();

                
                string[] queries = message.Split(';');

                ///W¹tek 1
                Thread oThread = new Thread(new ParameterizedThreadStart(
                oMyThreadClass.CreateClientOrder));
                oThread.Start(queries[0]);
                oThread.Join();
                if (oThread.IsAlive)
                {
                    oThread.Abort();
                }

                //kolejka - AddClient (na nowym watku)
                Thread oThread1 = new Thread(new ParameterizedThreadStart(
                oMyThreadClass.AddClient));
                oThread1.Start(queries[1]);
                oThread1.Join();
                if (oThread1.IsAlive)
                {
                    oThread1.Abort();
                }

                //query3
                //Thread oThread3 = new Thread(new ParameterizedThreadStart(
                //oMyThreadClass.AddOrderProduct));
                int i = 2;
                while ((i<queries.Length)&&(!queries[i].Equals("")))
                {
                    Thread oThread3 = new Thread(new ParameterizedThreadStart(
                oMyThreadClass.AddOrderProduct));
                    oThread3.Start(queries[i]);
                    oThread3.Join();
                    if (oThread3.IsAlive)
                    {
                        oThread3.Abort();
                    }
                    i++;
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
                        try
                        {

                            response = CallBuyingQueue(message, channel).ToString();

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

