using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Application.Common.Interfaces;
using SignalR.WebApi.Hubs;
using Xamarin.Forms.Internals;

namespace SignalR.WebApi.Services
{
    public class QueueService : IQueueService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly Queue<string> _queue = new Queue<string>();
        private readonly List<(string, List<string>)> _chatList = new List<(string, List<string>)>();
        
        public QueueService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public string Enqueue(string chatId, string connectionId)
        {
            try
            {
                _queue.Enqueue(chatId);
                
                var chat = _chatList.FirstOrDefault(c => c.Item1 == chatId);
                if (chat.Item1 == null)
                    _chatList.Add((chatId, new List<string>{connectionId}));
                else
                {
                    _chatList.Remove(chat);
                    chat.Item2.Add(connectionId);
                    _chatList.Add(chat);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
        
        public (string, string) Dequeue(string connectionId)
        {
            try
            {
                var success = _queue.TryDequeue(out var result);
                if (!success) return ("Not possible to dequeue", null);
                
                return (null, result);
            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }

        public string GetChatId(string connectionId)
        {
            return _chatList.FirstOrDefault(x => x.Item2.Any(z => z == connectionId)).Item1;
        }

        public async Task SendQueueNr()
        {
            foreach (var connectionId in _queue)
            {
                _chatList.FirstOrDefault(c => c.Item1 == connectionId).Item2.ForEach(async user => await _hubContext.Clients.Client(user).SendAsync("ReceiveQueueNr", _queue.ToList().IndexOf(connectionId)+1));
            }
        }

        public void WorkerSwitchChannel(string connectionId, string chatId)
        {
            foreach (var connection in _chatList.Where(c => c.Item2.Any(x => x == connectionId)))
            {
                connection.Item2.Remove(connectionId);
            }
            
            var chat = _chatList.FirstOrDefault(c => c.Item1 == chatId);
            _chatList.Remove(chat);
            chat.Item2.Add(connectionId);
            _chatList.Add(chat);
        }
        
        public int GetQueueCount()
        {
            return _queue.Count;
        }
        
        public async Task SendQueueCount()
        {
            await _hubContext.Clients.Group("Worker").SendAsync("ReceiveQueueCount", GetQueueCount());
        }
    }
}