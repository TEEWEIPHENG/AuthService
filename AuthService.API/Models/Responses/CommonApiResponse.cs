namespace AuthService.API.Models.Responses
{
    public class CommonApiResponse
    {
        public bool Success { get; set;  }
        public string Message { get; set; } = null!;
    }
}
