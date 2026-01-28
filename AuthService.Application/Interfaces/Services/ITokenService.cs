using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateToken(string userId, string username);
        public string GenerateRefreshToken();

    }
}
