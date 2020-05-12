using System;
using Syncfusion.XForms.Backdrop;
using XamarinApp.Views;

namespace XamarinApp
{
    public partial class Backdrop : SfBackdropPage
    {
        public Backdrop()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoginView());
        }
    }
}