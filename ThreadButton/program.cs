using System;
using Maoui;
using System.Threading;
using Xamarin.Forms;

namespace ThreadButton
{
    public class HelloWorld
    {
        public static void Main(String[] args)
        {
            var t = new Thread(delegate () {
                Console.WriteLine("In thread.");
                Thread.Sleep(1000);
            });
            t.Start();
            t.Join();
            Console.WriteLine("Hello, World!");
        }
    }

    class Program
    {
        static void Main(string[] args)
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
                var t = new Thread(delegate () {
                    Console.WriteLine("In thread.");
                    Thread.Sleep(1000);
                    count++;
                });
                t.Start();
                // t.Join();
                button.Text = $"Clicked {count} times";
            };
            UI.Publish("/", page.GetMaouiElement());
        }
    }
}