using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Infrastructure.Services;
using XamarinApp.ViewModels;
using XamarinApp.ViewModels.Resource;
using XamarinApp.ViewModels.User;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinApp
{
    public partial class App : Xamarin.Forms.Application, IHaveMainPage
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU2MjgxQDMxMzgyZTMxMmUzMFVFU0pIVWVHcm1OQ1dGRjNaS2IwZmlTeWMwak1IbUsyNTFteFdMKzlDelU9;MjU2MjgyQDMxMzgyZTMxMmUzMGlqVHJIY2pFQ2FUVDNxNmdOMXVaMWRhNnRoMjF5cCtDdkMybDBhb3ZLbkE9;MjU2MjgzQDMxMzgyZTMxMmUzMGhsMDRGQlZac3R3UVRBcFJ0MVRSTjd2bGgzdlRlSmdoVmY3UktUMndRaUE9;MjU2Mjg0QDMxMzgyZTMxMmUzMFBaaXQwT2MzT0w3UVRwampyZm1KYzllZ3lRb2tRV0VMVDh1R0JPYk40ZU09");
            
            InitializeComponent();
            
            var navigator = new NavigationService(this, new ViewLocator());

            var rootViewModel = new LoginViewModel(navigator, "/Login");

            navigator.PresentAsNavigatableMainPage(rootViewModel);
            
            // MainPage = new NavigationPage(new LoginPageCS())
            // {
            //     BarTextColor = Color.White,
            //     BarBackgroundColor = Color.FromHex("007DE6")
            // };
        }

        public static readonly string AppName = "XamarinApp"; 

        protected override void OnStart()
        {
            // Handle when your app starts
            
            SecureStorage.RemoveAll();
            
            Current.Properties["MobileBffUrl"] = "http://10.0.2.2:5000/";
            Current.Properties["SignalRUrl"] = "http://10.0.2.2:5012";
            //var id = Application.Current.Properties ["id"] as int;

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
