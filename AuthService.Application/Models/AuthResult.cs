using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Models
{
    public record AuthResult(string AccessToken,string RefreshToken,DateTime ExpiresAt);
}
