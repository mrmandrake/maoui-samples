using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Maoui;
using Maoui.Forms;

namespace BugSweeperPage
{
    public class Program
    {
        public static void Main()
        {
            Forms.Init();
            UI.Publish("/", new BugsPage().GetMaouiElement());
            // Maoui.UI.StartWebAssembly(double.Parse(initialWidth), double.Parse(InitialHeight);
        }
    }
}
