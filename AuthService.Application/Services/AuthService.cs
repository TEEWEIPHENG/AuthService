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

                PhoneNumber phoneNumber = PhoneNumber.Create(request.MobileNo);
                var user = new User(request.Username, email, passwordHash, role, request.Firstname, request.Lastname, phoneNumber);

                await _userRepository.CreateAsync(user);

                _logger.LogInformation("User registered successfully: {Email}", request.Email);
                var tokenResult = _tokenService.Generate(user);
                var refreshToken = _tokenService.RefreshToken();
                return AuthResult.Success(tokenResult.AccessToken, refreshToken, tokenResult.ExpiresAt);

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
            try
            {
                var user = await _userRepository.GetByUsernameAsync(request.Credential);

                // Always fake delay to prevent timing attack
                await Task.Delay(50);

                if (user == null)
                {
                    user = await _userRepository.GetByEmailAsync(request.Credential);
                    await Task.Delay(50);
                }
                
                if(user == null || user.IsDeleted)
                    return AuthResult.Fail("Invalid Credentials");

                if(user.IsLocked)
                    return AuthResult.Fail("Account locked");
                
                var result = _passwordHasher.Verify(request.Password, user.PasswordHash.Value);

                if(!result)
                {
                    user.IncrementFailedLogin();
                    await _userRepository.UpdateAsync(user);
                    return AuthResult.Fail("Invalid Credentials");
                }
                _logger.LogInformation("User login successfully: {Credential}", request.Credential);

                //reset failed attempts
                user.ResetFailedLogin();
                user.MarkLogin();
                await _userRepository.UpdateAsync(user);
                var tokenResult = _tokenService.Generate(user);
                var refreshToken = _tokenService.RefreshToken();
                return AuthResult.Success(tokenResult.AccessToken, refreshToken, tokenResult.ExpiresAt);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed when login {Credential}", request.Credential);
                return AuthResult.Fail(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when login for {Credential}", request.Credential);
                return AuthResult.Fail("Internal server error");
            }
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
        public async Task ResetPasswordAsync(string userId, string mfaToken, string newPassword)
        {
            try
            {
                _logger.LogInformation("User reset password {userId}", userId);
                //check if mfaToken valid with the userId

                //update new password
            }
            catch(DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed when login {Credential}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when reset password {Credential}");
            }
        }
    }
}
