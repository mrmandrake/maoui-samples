using System;
using Maoui;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Alert
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class DisplayAlertPage : ContentPage
    {
        public void InitializeComponent()
        {
            var btn = new Xamarin.Forms.Button() { Text = "tap to display alert" };
            btn.Clicked += OnButtonClicked;

            Content = new StackLayout()
            {
                Padding = 20,
                Children =
                {
                    new Xamarin.Forms.Label() { Text = "Welcome to DisplayAlert Sample!", FontSize = 32, FontAttributes = FontAttributes.Bold },
                    new ActivityIndicator() { },
                    new DatePicker(),
                    new StackLayout()
                    {
                        Children =
                        {
                            new Switch() {IsToggled = true },
                            new Switch() {IsToggled = false }
                        }
                    },
                    btn
                    
                }
            };
        }

        public DisplayAlertPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // status.Text = "Page appeared";
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Console.WriteLine("Disappear was called");
        }

        public async void OnButtonClicked(object sender, EventArgs args)
        {
            //activity.IsRunning = true;
            //progress.Progress = 0.5;
            var result = await DisplayAlert($"Alert ", "This is a test of the dialog. Is it working?", "YES", "NO");
            await DisplayAlert("Alert Response", $"You selected value: {result}", "OK");
            //activity.IsRunning = false;
            //progress.Progress = 1.0;
        }
    }
}
