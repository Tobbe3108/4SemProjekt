using System.Linq;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.DataForm;
using Xamarin.Forms;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    public class RegisterPageCS : ContentPage
    {
        private RegisterViewModel _registerViewModel;

        public RegisterPageCS()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            
            BindingContext = _registerViewModel
                = new RegisterViewModel();
            
            var title = new Label
            {
                Text = "Register",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            
            var dataForm = new SfDataForm
            {
                LayoutOptions = LayoutType.TextInputLayout,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ValidationMode = ValidationMode.PropertyChanged
            };
            dataForm.SetBinding(SfDataForm.DataObjectProperty, "RegisterInformation");
            
            var register = new SfButton
            {
                Text = "Register",
                BackgroundColor = (Color) Application.Current.Resources["primaryColor"]
            };
            register.Clicked += async (sender, e) =>
            {
                await Navigation.PushModalAsync(new BusyModalCS());
                if (await _registerViewModel.Register())
                {
                    await Navigation.PopModalAsync();
                    Navigation.InsertPageBefore(new HomePageCS(new ResourceListPageCS()),Navigation.NavigationStack[0]);
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await Navigation.PopModalAsync();
                    await DisplayAlert("Alert", _registerViewModel.ErrorMessage, "OK");
                }
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
                Children = {title, dataForm, register, back}
            };
        }
    }
}