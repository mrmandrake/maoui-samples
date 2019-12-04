using System;
using Maoui;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;
using Label = Xamarin.Forms.Label;
using System.Net.WebSockets;
using ClientWebSocket = WebAssembly.Net.WebSockets.ClientWebSocket;
using ClientHttp = WebAssembly.Net.Http.HttpClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WasmWebSocket
{
    class Program
    {
        static async void TestHttp()
        {
            Console.WriteLine("http call....");
            try
            {
                var client = new HttpClient();
                HttpContent httpContent = new StringContent("test", Encoding.UTF8, "application/json");
                var req = new HttpRequestMessage(HttpMethod.Post, new Uri("http://127.0.0.1:9301/xyz") { }) {
                    Content = httpContent
                };

                var resp = await client.SendAsync(new HttpRequestMessage());
                Console.WriteLine(resp.Content);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("....http call");
        }

        static void TestTask() {
            Console.WriteLine("Test Task....");
            try
            {
                Task.Factory.StartNew(() => {Console.WriteLine("Hello Task library!"); });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("....Task Test");            
        }

        static async void TestWS()
        {
            Console.WriteLine("ws call....");
            ClientWebSocket cws = new ClientWebSocket();
            //await var t =  cws.SendAsync(new Uri("ws://127.0.0.1:9301/ws"), CancellationToken.None);
            await cws.ConnectAsync(new Uri("ws://127.0.0.1:9301/ws"), CancellationToken.None);
            await cws.SendAsync(new ArraySegment<byte> (Encoding.UTF8.GetBytes("{test}")), WebSocketMessageType.Text, true, CancellationToken.None);
            var buffer = new ArraySegment<byte> (new byte [4096]);
            var result = await cws.ReceiveAsync(buffer, CancellationToken.None);
            Console.WriteLine("....ws call -> " + Encoding.UTF8.GetString(buffer));
        }

        static void TestThread()
        {
            Console.WriteLine("Thread Test ....");
            try
            {
                var task1 = new Thread(() =>
                {
                    Console.WriteLine("IN BTNCLICK THREAD!");
                    ClientWebSocket cws = new ClientWebSocket();
                    cws.ConnectAsync(new Uri("ws://127.0.0.1:9301/ws"), CancellationToken.None);
                });

                task1.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("....Thread Test");
        }
        static void Main()
        {
            Forms.Init();
            var page = new ContentPage();
            var stack = new StackLayout();
            var button = new Xamarin.Forms.Button
            {
                Text = "Click me!"
            };
            stack.Children.Add(button);
            page.Content = stack;
            var count = 0;
            button.Clicked += (s, e) =>
            {
                count++;
                button.Text = $"Clicked {count} times";

                //TestThread();
                TestHttp();
                TestWS();
                //TestTask();
            };
            UI.Publish("/", page.GetMaouiElement());
        }
    }
}