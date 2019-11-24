using System;
using Xamarin.Forms;
using Maoui;
using Maoui.Forms;

namespace MatrixClock
{
    class Program
    {
        public static void Main()
        {
            Forms.Init();
            UI.Publish("/", new DotMatrixClockPage().GetMaouiElement());
        }
    }
}
