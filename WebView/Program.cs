using System;
using Xamarin.Forms;

namespace WebView
{
    class Program
    {
        public static Maoui.Element CreateElement()
        {
            var panel = new StackLayout();

            var titleLabel = new Xamarin.Forms.Label
            {
                Text = "WebView",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold
            };
            panel.Children.Add(titleLabel);

            Xamarin.Forms.WebView webview = new Xamarin.Forms.WebView
            {
                Source = "http://www.xamarin.com"
            };
            panel.Children.Add(webview);

            var page = new ContentPage
            {
                Content = panel
            };

            return page.GetMaouiElement();
        }

        public static void Main()
        {
            Forms.Init();
            Maoui.UI.Publish("/", CreateElement());
        }
    }
}
