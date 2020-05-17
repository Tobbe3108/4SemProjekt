using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.DataForm;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.User;

namespace XamarinApp.Views.User
{
    public class LoginView : ContentPage
    {
        private readonly LoginViewModel _loginViewModel;

        public LoginView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            NavigationPage.SetHasNavigationBar(this, false);
            
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
                ValidationMode = ValidationMode.PropertyChanged,
                DataObject = ((LoginViewModel)BindingContext).LoginInformation
            };
            
            var login = new SfButton
            {
                Text = "Login",
                BackgroundColor = (Color) Xamarin.Forms.Application.Current.Resources["primaryColor"],
                Command = ((LoginViewModel)BindingContext).NavigateToResourceListPageCommand
            };
            
            var signup = new SfButton
            {
                Text = "Signup",
                BackgroundColor = Color.Transparent,
                TextColor = Color.Gray,
                Command = ((LoginViewModel)BindingContext).NavigateToRegisterPageCommand
            };

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