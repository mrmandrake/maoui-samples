using System;
using Xamarin.Forms;

namespace ButtonXaml
{
    class Program
    {
        public static void Main()
        {
            Forms.Init();
            Maoui.UI.Publish("/", new ButtonXamlPage().GetMaouiElement());
        }
    }
}
