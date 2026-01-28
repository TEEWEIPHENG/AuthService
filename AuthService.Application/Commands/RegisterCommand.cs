using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Commands
{
    public sealed class RegisterCommand
    {
        public string Email { get; init; } = default!;
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
