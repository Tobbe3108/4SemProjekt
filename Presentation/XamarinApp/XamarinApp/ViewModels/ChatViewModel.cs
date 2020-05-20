using System.Collections.ObjectModel;
using Syncfusion.XForms.Chat;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;

namespace XamarinApp.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        #endregion

        public ObservableCollection<object> Messages;
        public Author CurrentUser;
        
        public ChatViewModel(INavigationService navigator, string navigationPath)
        {
            #region Navigation
            NavigationPath = navigationPath;
            _navigator = navigator;
            #endregion

            Messages = new ObservableCollection<object>();
            CurrentUser = new Author
            {
                Name = new UserViewModel().User.Username
            };
        }
        
        public void MessageSent (object sender, SendMessageEventArgs e)
        {
            //throw new System.NotImplementedException();
        }
    }
}