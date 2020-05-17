using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.DataForm;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.User;

namespace XamarinApp.Views.User
{
    public class RegisterView : ContentPage
    {
        private RegisterViewModel _registerViewModel;

        public RegisterView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            NavigationPage.SetHasNavigationBar(this, false);
            
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
                ValidationMode = ValidationMode.PropertyChanged,
                DataObject = ((RegisterViewModel)BindingContext).RegisterInformation
            };
            
            var register = new SfButton
            {
                Text = "Register",
                BackgroundColor = (Color) Xamarin.Forms.Application.Current.Resources["primaryColor"],
                Command = ((RegisterViewModel)BindingContext).NavigateToResourceListPageCommand
            };
            
            var back = new SfButton
            {
                Text = "Back",
                BackgroundColor = Color.Transparent,
                TextColor = Color.Gray,
                Command = ((RegisterViewModel)BindingContext).NavigateBack
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