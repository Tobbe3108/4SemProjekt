using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        private readonly LoginViewModel _loginViewModel;

        public LoginView()
        {
            InitializeComponent();
            
            BindingContext = _loginViewModel
                = new LoginViewModel();
            
            var title = new Label {
                Text = "Login",
                FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            
            var email = new Entry {
                Placeholder = "Email or Username"
            };
            email.SetBinding(Entry.TextProperty, "UsernameOrEmail");

            var password = new Entry
            {
                Placeholder = "Password",
                IsPassword = true
            };
            password.SetBinding(Entry.TextProperty, "Password");

            var login = new Button {
                Text = "Login"
            };
            
            // var signupButton = new Button {
            //     Text = "Signup"
            // };
            //
            // // Navigation to the Signup Page (Note: We haven't created this page yet)
            // signupButton.Clicked += (object sender, EventArgs e) => {
            //     Navigation.PushAsync(new SignupPage());
            // };
            
            login.Clicked += async (sender, e) =>
            {
                if (await _loginViewModel.Login())
                    await Navigation.PopModalAsync();
                else
                    await DisplayAlert("Alert", _loginViewModel.ErrorMessage, "OK");
            };

            Content = new StackLayout {
                Padding = 30,
                Spacing = 10,
                Children = {title, email, password, login, /*signupButton*/}
            };
            
        }
    }
}