using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.DataForm;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.User;

namespace XamarinApp.Views.User
{
    public class ProfileView : ContentPage
    {
        public ProfileView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            NavigationPage.SetHasNavigationBar(this, false);

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
                ValidationMode = ValidationMode.PropertyChanged,
                DataObject = ((ProfileViewModel)BindingContext).ProfileInformation
            };

            var update = new SfButton
            {
                Text = "Update",
                BackgroundColor = (Color) Xamarin.Forms.Application.Current.Resources["primaryColor"],
                Command = ((ProfileViewModel)BindingContext).UpdateCommand
            };

            var back = new SfButton
            {
                Text = "Back",
                BackgroundColor = Color.Transparent,
                TextColor = Color.Gray,
                Command = ((ProfileViewModel)BindingContext).NavigateBack
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