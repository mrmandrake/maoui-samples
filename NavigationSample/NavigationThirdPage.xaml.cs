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
	public partial class NavigationThirdPage : ContentPage
	{
		public NavigationThirdPage ()
		{
			this.InitializeComponent();
		}

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            (this.Parent as NavigationPage).PopAsync(false);
        }

        private void RootButton_Clicked(object sender, EventArgs e)
        {
            (this.Parent as NavigationPage).PopToRootAsync(false);
        }
    }
}
