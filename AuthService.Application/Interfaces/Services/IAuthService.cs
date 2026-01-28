using AuthService.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthResult> LoginAsync(LoginRequest request);
        public Task<AuthResult> RegisterAsync(RegisterRequest request);
        public Task LogoutAsync(string userId);
        public Task<AuthResult> RefreshTokenAsync(RefreshTokenRequest request);
        public Task<UserProfileDto> GetProfileAsync(string userId);
        public Task ChangePasswordAsync(string newPassword);

    }
}
