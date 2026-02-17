using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces.Services
{
    public interface ITokenService
    {
        TokenResult Generate(User user);

        string RefreshToken();
    }

    public record TokenResult(string AccessToken, DateTime ExpiresAt);
}
