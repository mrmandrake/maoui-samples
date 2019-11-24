using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace RefreshListView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class RefreshListView : ContentPage
    {
        public RefreshListView()
        {
            BindingContext = new RefreshListViewModel();
        }

        public void Handle_Clicked(object sender, System.EventArgs args)
        {
            string item = ((RefreshListViewModel)BindingContext).Data.LastOrDefault();
            this.FindByName<ListView>("list").ScrollTo(item, ScrollToPosition.End, true);
        }

        public static RefreshListView InstanceFromXaml()
        {
            var xaml = @"<ContentPage xmlns =""http://xamarin.com/schemas/2014/forms"" xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml"" x:Class=""Samples.RefreshListView"">
            <ContentPage.Content>
                <StackLayout> 
                    <ListView x:Name =""list"" ItemsSource =""{Binding Data}"" HeightRequest =""200"">
                        <ListView.ItemTemplate><DataTemplate><ViewCell><Label Text =""{Binding .}"" /></ViewCell></DataTemplate></ListView.ItemTemplate>
                    </ListView>
                    <StackLayout>
                        <Entry Text =""{Binding Input}"" />
                        <Button Text =""Add Item"" Command =""{Binding AddCmd}"" Clicked=""Handle_Clicked""/>
                   </StackLayout>
               </StackLayout>
           </ContentPage.Content>
           </ContentPage>";

            var instance = new RefreshListView();
            instance.LoadFromXaml<RefreshListView>(xaml);
            return instance;
        }
    }
}
