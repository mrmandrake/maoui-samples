using Maoui;
using Xamarin.Forms;
using WebAssembly;

namespace WebView
{   
    public class Program
    {       
        public static void Main()
        {
            Forms.Init();
            var page = new ContentPage();
            var stack = new StackLayout();
            var button = new Xamarin.Forms.Button { Text = "Click me!" };
            stack.Children.Add(button);
            page.Content = stack;
            // var count = 0;
            button.Clicked += (s, e) => {
                var window = (JSObject)Runtime.GetGlobalObject("window");
                var location = (JSObject)window.GetObjectProperty("location");
                // window.Invoke()

                WebAssembly.Runtime.InvokeJS(@"window.localStorage.setItem('user', 'test');");
                var result = WebAssembly.Runtime.InvokeJS(@"window.localStorage.getItem('user');");
                button.Text = result;
            };
            
            UI.Publish("/", page.GetMaouiElement());
        }
    }
}