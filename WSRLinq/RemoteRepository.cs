using Aqua.Dynamic;
using Newtonsoft.Json;
using Remote.Linq;
using Remote.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSRLinq
{
    public class RemoteRepository
    {
        private readonly Func<Expression, Task<IEnumerable<DynamicObject>>> _dataProvider;

        public static JsonSerializerSettings serializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }.ConfigureRemoteLinq();

        public static WSHelper ws = new WSHelper();

        public ClientWebSocket cws = null;

        public class OrderItem
        {
            public int Id { get; set; }

            public int ProductId { get; set; }

            public int Quantity { get; set; }
        }

        public RemoteRepository()
        {
            cws = WSHelper.CreateWebSocket();

            _dataProvider = async expression =>
            {
                IEnumerable<DynamicObject> result = null;
                try
                {
                    try
                    {
                        var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                        Console.WriteLine("connection");
                        await cws.ConnectAsync(new Uri("ws://127.0.0.1:9301/ws"), CancellationToken.None);
                        Console.WriteLine("sending");
                        await cws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("test")), WebSocketMessageType.Text, true, CancellationToken.None);
                        Console.WriteLine("receving");
                        // var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);

                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine($"{exc.Message} / {exc.InnerException.Message}");
                    }
                    // Console.WriteLine("remote repo:sending to ws://127.0.0.1:9301/ws");
                    // string json = Newtonsoft.Json.JsonConvert.SerializeObject(expression, serializerSettings);
                    // Console.WriteLine("serializiation:" + json);
                    // Console.WriteLine("pre:");
                    // await ws.WebSocketSendText(json);
                    // var reply = await ws.ReceiveHostCloseWebSocket();
                    // System.Threading.Thread.Sleep(1000);
                    // var reply = await ws.WebSocketRecvText();
                    // Console.WriteLine($"reply: {reply}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return result;

                //entry.Text = recv;
                //using (var client = new TcpClient(server, port))
                //{
                //    using (var stream = client.GetStream())
                //    {
                //        stream.Write(expression);
                //        result = stream.Read<IEnumerable<DynamicObject>>();
                //        stream.Close();
                //    }

                //    client.Close();
                //}

                // return result;
            };
        }

        //public IQueryable<ProductCategory> ProductCategories => RemoteQueryable.Factory.CreateQueryable<ProductCategory>(_dataProvider);

        //public IQueryable<Product> Products => RemoteQueryable.Factory.CreateQueryable<Product>(_dataProvider);

        public IQueryable<OrderItem> OrderItems => RemoteQueryable.Factory.CreateQueryable<OrderItem>(_dataProvider);
    }
}