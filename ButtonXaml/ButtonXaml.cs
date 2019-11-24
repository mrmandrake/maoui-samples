using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ButtonXaml
{

    public class ButtonXamlPage : ContentPage
    {
        int count = 0;

        public Entry entry;

        RemoteRepository repo = new RemoteRepository();

        public ButtonXamlPage()
        {
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            entry = new Entry() { Text = "Click Count: 0" };
            var btn = new Xamarin.Forms.Button() { Text = "Pressme" };
            btn.Clicked += OnButtonClicked;
            Content = new StackLayout()
            {
                Padding = 20,
                Children =
                {
                    new Label() { Text = "Welcome!", FontSize = 32, FontAttributes = FontAttributes.Bold },
                    entry,
                    btn
                }
            };
        }

        public async void OnButtonClicked(object sender, EventArgs args)
        {
            entry.Text = $"Click Count: {++count}";
            try
            {
                var cws = WSHelper.CreateWebSocket();
                var rcvBuffer = new ArraySegment<byte>(new byte[4096]);
                Console.WriteLine("connection");
                await cws.ConnectAsync(new Uri("ws://127.0.0.1:9301/ws"), CancellationToken.None);
                Console.WriteLine("sending");
                await cws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("test")), WebSocketMessageType.Text, true, CancellationToken.None);
                var r = await cws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                Console.WriteLine("receving:" + rcvBuffer);
                // Console.WriteLine("sending to ws://127.0.0.1:9301/ws");
                // // var state = await RemoteRepository.ws.ConnectWebSocket(new Uri("ws://127.0.0.1:9301/ws"), null);
                // Console.WriteLine("btnclick");
                // foreach (var item in repo.OrderItems)
                //     Console.WriteLine($"item: {item.Id} {item.ProductId} {item.ProductId}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
