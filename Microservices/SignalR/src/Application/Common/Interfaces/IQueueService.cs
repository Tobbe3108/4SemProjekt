using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Application.Common.Interfaces
{
    public interface IQueueService
    {
        string Enqueue(string chatId, string connectionId);
        (string, string) Dequeue(string connectionId);
        string GetChatId(string connectionId);
        Task SendQueueNr();
        int GetQueueCount();
        Task SendQueueCount();
        void WorkerSwitchChannel(string connectionId, string chatId);
    }
}