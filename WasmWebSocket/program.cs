using System;
using Maoui;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;
using Label = Xamarin.Forms.Label;

namespace WebView
{
 
    class Program
    {
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
            };
            UI.Publish("/", page.GetMaouiElement());
        }
    }
}