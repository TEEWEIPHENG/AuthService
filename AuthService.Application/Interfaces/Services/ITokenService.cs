using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces.Services
{
    public interface ITokenService
    {
        TokenResult Generate(User user);

    }

    public record TokenResult(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}
