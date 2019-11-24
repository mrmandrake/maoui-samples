using System;
using Maoui;
using Xamarin.Forms;

namespace Alert
{
    class Program
    {
        public static void Main()
        {
            Forms.Init();            
            Maoui.UI.Publish("/", new DisplayAlertPage().GetMaouiElement());
        }
    }
}
