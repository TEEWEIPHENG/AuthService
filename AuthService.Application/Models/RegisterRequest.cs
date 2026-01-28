using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Models
{
    public record RegisterRequest(string Username,string Email,string Password);
}
