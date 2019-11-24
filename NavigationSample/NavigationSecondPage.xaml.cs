using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maoui.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NavigationSample
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavigationSecondPage : ContentPage
	{
		public NavigationSecondPage ()
		{
			this.InitializeComponent();
		}

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            (this.Parent as NavigationPage).PopAsync();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            (this.Parent as NavigationPage).PushAsync(new NavigationThirdPage());
        }
    }
}
