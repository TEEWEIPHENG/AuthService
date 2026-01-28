using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Models
{
    public record LoginRequest(string Username,string Password);
}
