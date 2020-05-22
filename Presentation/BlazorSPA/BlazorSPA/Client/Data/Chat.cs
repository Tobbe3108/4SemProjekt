using System;
using System.Collections.Generic;

namespace BlazorSPA.Client.Data
{
    public class Chat
    {
        public string ChatId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        
        public List<Message> Messages { get; set; }
    }
}