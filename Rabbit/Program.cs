using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFServiceWebRole1;
using System.Threading;
using System.Net.Mail;
using System.Net;

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

        public bool CreateClientOrder(object data)
        {
            if (data.ToString().Equals("")) return false;
            string[] parameters = data.ToString().Split(',');
            order_id = service.CreateClientOrder(parameters[0], parameters[1]).ToString();
            if (Int32.Parse(order_id) != 0) return true;
            else return false;

        }
         public bool AddClient(object data)
        {
            if (data.ToString().Equals("")) return false;
            string[] parameters = data.ToString().Split(',');
            var response2 = service.AddClient(parameters[0], parameters[1], order_id).ToString();
            if (response2.Equals("True")) return true;
            else return false;
        }
        public bool AddOrderProduct(object data)
        {
            //if (data.ToString().Equals("")) return false;
            //string[] parameters = data.ToString().Split(',');
            //var response3 = service.AddOrderProduct(parameters[0], parameters[1], order_id);
            //if (response3.Equals("True")) return true;
            //else return false;
            return false;


        }
        public bool BuyProduct(object data)
        {
            if (data.ToString().Equals("")) return false;
            string[] parameters = data.ToString().Split(',');
            Console.WriteLine(" [.] buy product ({0})", parameters[0]);
            var response3 = service.BuyProduct(parameters[0], parameters[1]);
            Console.WriteLine("Buy product: " + response3.ToString());
            if (response3.Equals("True")) return true;
            else return false;
        }
    }
    class Program
    {

         static string Message(string all)
        {
            string[] products = all.Split(';');
            string body = "Hi! Thank you for order in our store! \n Your order includes: \n\n";
            foreach (var product in products)
            {
                string[] thing = product.Split(',');
                body += thing[0] + " \n";
                body += "\b\b Amount: " + thing[1];
                body += " \n ";
            }
            body += "\n\n We hope you will be satisfied with our services. \n Greetings, \n Team WebStore";
            Console.WriteLine(body);
            return body;
        }
        static void Send(string address, string products)
        {
            var fromAddress = new MailAddress("chatwithmedev@gmail.com", "From Name");
            var toAddress = new MailAddress(address.ToString(), "To Name");
            const string fromPassword = "cwmdev22";
            const string subject = "Your order from WebStore";

            string body = Message(products);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private static bool CallBuyingQueue(string message, IModel channel)
        {
            try
            {
                OrderThreadClass oMyThreadClass = new OrderThreadClass();

                
                string[] queries = message.Split(';');
                var mail = queries[0];
                ///W�tek 1
                string a = queries[1] + "," + queries[0];

                bool response1 = false;
                Thread oThread = new Thread(delegate() 
                {  response1 = oMyThreadClass.CreateClientOrder(a); });
                   // if (response == false) if_break=true;
                
                oThread.Start();
                oThread.Join();
                if (response1 == false) return false;
                if (oThread.IsAlive)
                {
                    oThread.Abort();
                }

                //kolejka - AddClient (na nowym watku)
                bool response2 = false;
                Thread oThread1 = new Thread(() =>
                { response2 = oMyThreadClass.CreateClientOrder(a); });

                oThread1.Start();
                oThread1.Join();
                if (response2 == false) return false;
                if (oThread1.IsAlive)
                {
                    oThread1.Abort();
                }
                //query3
                //Thread oThread3 = new Thread(new ParameterizedThreadStart(
                //oMyThreadClass.AddOrderProduct));
                int i = 3;
                string products="";
                while ((i < queries.Length) && (!queries[i].Equals("")))
                {
                    bool response3 = false;
                    Thread oThread3 = new Thread(() =>
                    { response3 = oMyThreadClass.CreateClientOrder(a); });
                    oThread3.Start();
                    oThread3.Join();
                    if (response3 == false)
                        return false;
                    if (oThread3.IsAlive)
                    {
                        oThread3.Abort();
                    }
                    i++;
                    //buy product   
                    bool response4 = false;
                    Thread oThread4 = new Thread(() => 
                    { response4 = oMyThreadClass.CreateClientOrder(a); });
                    oThread4.Start();
                    oThread4.Join();
                    if (response4 == false) return false;
                    if (oThread4.IsAlive)
                    {
                        oThread4.Abort();
                    }
                    products += queries[i];
                    i++;
                }
                 Send(mail, products); //wysli do kolejki chec send_email
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

