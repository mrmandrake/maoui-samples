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

        public void OnButtonClicked(object sender, EventArgs args)
        {
            entry.Text = $"Click Count: {++count}";
            Console.WriteLine("btnclick");

            Console.WriteLine("IN BTNCLICK THREAD!");
            ClientWebSocket cws = new ClientWebSocket();
            cws.ConnectAsync(new Uri("ws://127.0.0.1:9301/ws"), CancellationToken.None);

            var t = new Thread(() => {
                try
                {
                    // foreach (var item in repo.OrderItems)
                    //     Console.WriteLine($"item: {item.Id} {item.ProductId} {item.ProductId}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }
    }
}
