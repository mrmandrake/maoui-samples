using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RefreshListView
{
    class Program
    {
        public static void Main()
        {
            Forms.Init();
            Maoui.UI.Publish("/", RefreshListView.InstanceFromXaml().GetMaouiElement());
        }
    }
}