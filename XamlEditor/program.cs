using System;
using Maoui;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamlEditor;

namespace XamlEditor
{
    class Program
    {       
        static void Main()
        {
            Forms.Init();
            var page = new XamlEditorPage();
            UI.Publish("/", page.GetMaouiElement());
        }
    }
}