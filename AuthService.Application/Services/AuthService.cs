using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Services;
using AuthService.Application.Models;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Register failed: email already exists: {Email}", request.Email);
                    return AuthResult.Fail("Email already in use");
                }

                Email email = Email.Create(request.Email);

                User.ValidatePassword(request.Password);
                var hashString = _passwordHasher.Hash(request.Password);
                PasswordHash passwordHash = PasswordHash.Create(hashString);

                Role role = Role.Create("User");

                var user = new User(request.Username, email, passwordHash, role, request.Firstname, request.Lastname);

                await _userRepository.CreateAsync(user);

                _logger.LogInformation("User registered successfully: {Email}", request.Email);
                var tokenResult = _tokenService.Generate(user);
                return AuthResult.Success(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.ExpiresAt);

            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed during registration for {Email}", request.Email);
                return AuthResult.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for {Email}", request.Email);
                return AuthResult.Fail("Internal server error");
            }
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
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

        public async Task ChangePasswordAsync(string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
