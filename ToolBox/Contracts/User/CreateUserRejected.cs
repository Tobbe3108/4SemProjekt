using System;

namespace ToolBox.Contracts.User
{
    public interface CreateUserRejected
    {
        public string Reason { get; set; }
    }
}