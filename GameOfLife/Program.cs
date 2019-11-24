using System;
using Maoui;
using Xamarin.Forms;

namespace GameOfLife
{
    public class Program
    {
        public static void Main() {
            Forms.Init();
            UI.Publish("/", new MainPage().GetMaouiElement());                
        }
    }
}
