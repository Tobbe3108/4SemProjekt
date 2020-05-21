using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Application.Common.Interfaces
{
    public interface IQueueService
    {
        string Enqueue(string chatId, string connectionId);
        (string, (string, string)) Dequeue();
        string GetChatId(string connectionId);
        Task SendQueueNr();
    }
}