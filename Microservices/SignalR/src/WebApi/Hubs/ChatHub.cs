using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Application.Common.Interfaces;
using SignalR.Domain.Entities;
using SignalR.WebApi.Services;

namespace SignalR.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IQueueService _queueService;

        public ChatHub(IQueueService queueService)
        {
            _queueService = queueService;
        }
        
        public async Task Enqueue(Guid chatId)
        {
            var message = _queueService.Enqueue(chatId.ToString(), Context.ConnectionId);
            if (message != null)
            {
                await Clients.Caller.SendAsync("Error", message); // Send error message to user
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await _queueService.SendQueueNr();
        }
        
        public async Task Dequeue()
        {
            var (message, result) = _queueService.Dequeue();
            if (message != null)
            {
                await Clients.Caller.SendAsync("Error", message); // Send error message to admin
                return;
            }
            
            await Clients.Caller.SendAsync("ReceiveChatId", result);
            await _queueService.SendQueueNr();
        }

        public async Task SendMessage(Message message)
        {
            var chatId = _queueService.GetChatId(Context.ConnectionId);
            await Clients.OthersInGroup(chatId).SendAsync("ReceiveMessage", message);
        }
    }
}