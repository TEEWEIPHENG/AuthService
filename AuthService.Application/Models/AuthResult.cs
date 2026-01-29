namespace AuthService.Application.Models
{
    public class AuthResult
    {
        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; private set; } = Array.Empty<string>();
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? ExpiresAt { get; private set; }

        public static AuthResult Success(string accessToken, string refreshToken, DateTime expiresAt) =>
            new AuthResult
            {
                Succeeded = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt
            };

        public static AuthResult Fail(params string[] errors) => 
            new AuthResult
            {
                Succeeded = false,
                Errors = errors
            };
    }
}
