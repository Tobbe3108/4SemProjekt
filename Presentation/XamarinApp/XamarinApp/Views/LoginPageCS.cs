using System;
using System.Net.Http.Headers;
using Flurl.Http.Testing;
using Syncfusion.SfBusyIndicator.XForms;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.DataForm;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    public class LoginPageCS : ContentPage
    {
        private readonly LoginViewModel _loginViewModel;

        public LoginPageCS()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            
            BindingContext = _loginViewModel
                = new LoginViewModel();

            var title = new Label
            {
                Text = "Login",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0,50)
            };
            
            var dataForm = new SfDataForm
            {
                LayoutOptions = LayoutType.TextInputLayout,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ValidationMode = ValidationMode.PropertyChanged
            };
            dataForm.SetBinding(SfDataForm.DataObjectProperty, "LoginInformation");
            
            var login = new SfButton
            {
                Text = "Login",
                BackgroundColor = (Color) Application.Current.Resources["primaryColor"]
            };
            
            login.Clicked += async (sender, e) =>
            {
                await Navigation.PushModalAsync(new BusyModalCS());
                if (await _loginViewModel.Login())
                {
                    await Navigation.PopModalAsync();
                    Navigation.InsertPageBefore(new HomePageCS(new ResourceListPageCS()),this);
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await Navigation.PopModalAsync();
                    await DisplayAlert("Alert", _loginViewModel.ErrorMessage, "OK");
                }
            };
            
            var signup = new SfButton
            {
                Text = "Signup",
                BackgroundColor = Color.Transparent,
                TextColor = Color.Gray,
            };
            signup.Clicked += (object sender, EventArgs e) => { Navigation.PushAsync(new RegisterPageCS()); };
            
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 30,
                Spacing = 10,
                Children = {title, dataForm, login, signup}
            };
        }

    }
}