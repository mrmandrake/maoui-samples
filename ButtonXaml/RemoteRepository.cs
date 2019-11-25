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
using WebAssembly;
using ClientWebSocket = WebAssembly.Net.WebSockets.ClientWebSocket;

namespace ButtonXaml
{
    public class RemoteRepository
    {
        private readonly Func<Expression, Task<IEnumerable<DynamicObject>>> _dataProvider;

        public static JsonSerializerSettings serializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }.ConfigureRemoteLinq();

        public class OrderItem
        {
            public int Id { get; set; }

            public int ProductId { get; set; }

            public int Quantity { get; set; }
        }

        public RemoteRepository()
        {
            ClientWebSocket cws = new ClientWebSocket();
            cws.ConnectAsync(new Uri("ws://127.0.0.1:9301/ws"), CancellationToken.None);
            _dataProvider = async expression =>
            {
                IEnumerable<DynamicObject> result = null;
                try
                {
                    // Console.WriteLine("remote repo:sending to ws://127.0.0.1:9301/ws");
                    // string json = Newtonsoft.Json.JsonConvert.SerializeObject(expression, serializerSettings);
                    // Console.WriteLine("serializiation:" + json);
                    // Console.WriteLine("pre:");
                    // await cws.Sendas(json);
                    // var reply = await cws.ReceiveHostCloseWebSocket();
                    // System.Threading.Thread.Sleep(1000);
                    // reply = await cws.WebSocketRecvText();
                    // Console.WriteLine($"reply: {reply}");
                    Console.WriteLine("inthread!!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return result;
            };
        }

        //public IQueryable<ProductCategory> ProductCategories => RemoteQueryable.Factory.CreateQueryable<ProductCategory>(_dataProvider);

        //public IQueryable<Product> Products => RemoteQueryable.Factory.CreateQueryable<Product>(_dataProvider);

        public IQueryable<OrderItem> OrderItems => RemoteQueryable.Factory.CreateQueryable<OrderItem>(_dataProvider);
    }
}