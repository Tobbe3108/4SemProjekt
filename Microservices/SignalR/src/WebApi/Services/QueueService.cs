using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Application.Common.Interfaces;
using SignalR.WebApi.Hubs;

namespace SignalR.WebApi.Services
{
    public class QueueService : IQueueService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly Queue<(string, string)> _queue = new Queue<(string, string)>();
        
        public QueueService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public string Enqueue(string chatId, string connectionId)
        {
            try
            {
                _queue.Enqueue((chatId, connectionId));
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
        
        public (string, (string, string)) Dequeue()
        {
            try
            {
                var success = _queue.TryDequeue(out var result);
                if (!success) return ("Not possible to dequeue", (null,null));
                return (null, result);
            }
            catch (Exception e)
            {
                return (e.Message, (null, null));
            }
        }

        public string GetChatId(string connectionId)
        {
            return _queue.FirstOrDefault(x => x.Item2 == connectionId).Item1;
        }

        public async Task SendQueueNr()
        {
            foreach (var user in _queue)
            {
                await _hubContext.Clients.Client(user.Item2).SendAsync("ReceiveQueueNr", _queue.ToList().IndexOf(user)+1);
            }
        }
    }
}