using Syncfusion.XForms.Chat;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    public class ChatView : ContentPage
    {
        public ChatView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;

            var chat = new SfChat
            {
                Messages = ((ChatViewModel) BindingContext).Messages,
                CurrentUser = ((ChatViewModel) BindingContext).CurrentUser,
                ShowOutgoingMessageTimestamp = true,
                ShowTypingIndicator = false,
                ShowIncomingMessageAuthorName = true,
                ShowIncomingMessageTimestamp = true,
                ShowTimeBreak = true,
                IncomingMessageTimestampFormat = "HH:mm",
                OutgoingMessageTimestampFormat = "HH:mm"
            };
            chat.SendMessage += (sender, args) => ((ChatViewModel) BindingContext).MessageSent(sender, args);
            
            var stackLayout = new StackLayout
            {
                Spacing = 10,
                Children = { chat }
            };
            
            Content = stackLayout;
        }
    }
}