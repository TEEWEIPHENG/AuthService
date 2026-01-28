using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces.Services;

using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Models;
using AuthService.Domain.Entities;

namespace AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository users,IPasswordHasher hasher,ITokenService tokenService)
        {
            _users = users;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        private AuthResult CreateAuthResult(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfileDto> GetProfileAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task LogoutAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
