using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.DataForm;
using Xamarin.Forms;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    public class ProfilePageCS : ContentPage
    {
        private readonly ProfileViewModel _profileViewModel;
        public ProfilePageCS()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            
            BindingContext = _profileViewModel 
                = new ProfileViewModel();
            
            var title = new Label
            {
                Text = "Profile",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            
            var dataForm = new SfDataForm
            {
                LayoutOptions = LayoutType.TextInputLayout,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ValidationMode = ValidationMode.PropertyChanged
            };
            dataForm.SetBinding(SfDataForm.DataObjectProperty, "ProfileInformation");
            
            var update = new SfButton
            {
                Text = "Update",
                BackgroundColor = (Color) Application.Current.Resources["primaryColor"]
            };
            update.Clicked += async (sender, e) =>
            {
                if (await _profileViewModel.Update())
                {
                    Navigation.InsertPageBefore(new HomePageCS(new ResourceListPageCS()),Navigation.NavigationStack[0]);
                    Navigation.RemovePage(Navigation.NavigationStack[1]);
                }
                else
                    await DisplayAlert("Alert", _profileViewModel.ErrorMessage, "OK");
            };
            
            var back = new SfButton
            {
                Text = "Back",
                BackgroundColor = Color.Transparent,
                TextColor = Color.Gray,
            };
            back.Clicked += async (sender, e) =>
            {
                await Navigation.PopAsync();
            };
            
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 30,
                Spacing = 10,
                Children = {title, dataForm, update, back}
            };
        }
    }
}