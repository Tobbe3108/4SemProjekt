using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Syncfusion.XForms.Chat;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;

namespace XamarinApp.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        #endregion

        
        private Guid _ticketId = Guid.NewGuid();
        public ObservableCollection<object> Messages;
        public Author CurrentUser;
        public ChatTypingIndicator TypingIndicator;
        HubConnection _hubConnection;
        
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
        
        public override async Task BeforeFirstShown()
        {
            #region SignalR
            var signalRUrl = Xamarin.Forms.Application.Current.Properties["SignalRUrl"] as string;
            _hubConnection = new HubConnectionBuilder().WithUrl($"{signalRUrl}/chatHub").Build();  
  
            _hubConnection.On<Message>("ReceiveMessage", (message) =>
            {
                Messages.Add(message);
            });  

            _hubConnection.On<int>("ReceiveQueueNr", (nr) =>
            {
                var message = (Message)Messages.FirstOrDefault(x => ((Message) x).Id == _ticketId); 
                if (message != null)
                    message.Text = $"You are Nr: {nr} in the queue";
                else
                    Messages.Add(new Message
                    {
                        Id = _ticketId,
                        Text = $"You are Nr: {nr} in the queue",
                        Author = new Author
                        {
                            Name = "ChatBot"
                        },
                        DateTime = DateTime.Now
                    });
            });
            
            _hubConnection.On<string>("Error", (error) =>
            {
                _navigator.DisplayAlert("Error!", error, "Ok");
            });

            await _hubConnection.StartAsync();
            await Enqueue;
            #endregion
        }

        private Task Enqueue => _hubConnection.SendAsync("Enqueue", _ticketId);
        
        public void MessageSent (object sender, SendMessageEventArgs e)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                Text = e.Message.Text,
                Author = e.Message.Author,
                DateTime = e.Message.DateTime
            };
            _hubConnection.SendAsync("SendMessage", message);
        }
    }
}