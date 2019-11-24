using System;
using Maoui;
using Maoui.Forms;
using Xamarin.Forms;

namespace NavigationSample
{
    public class Program
    {
        public static Maoui.Element CreateElement()
        {
            var page = new NavigationFirstPage();
            var root = new NavigationPage(page);
            return root.GetMaouiElement();
        }

        public static void Main()
        {
            Forms.Init();
            UI.Publish("/", CreateElement());
        }
    }
}
