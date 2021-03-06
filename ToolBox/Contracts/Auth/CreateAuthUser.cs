﻿using System;

namespace ToolBox.Contracts.Auth
{
    public interface CreateAuthUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}