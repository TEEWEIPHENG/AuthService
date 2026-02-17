using AuthService.Application.Models;

namespace AuthService.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthResult> RegisterAsync(RegisterRequest request);
        public Task<AuthResult> LoginAsync(LoginRequest request);
        public Task LogoutAsync(string userId);
        public Task<AuthResult> RefreshTokenAsync(RefreshTokenRequest request);
        public Task<UserProfileDto> GetProfileAsync(string userId);
        public Task ChangePasswordAsync(string newPassword);
        public Task ResetPasswordAsync(string userId, string mfaToken, string newPassword);

    }
}
