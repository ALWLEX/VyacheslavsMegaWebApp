namespace VyacheslavsMegaWebApp.Api.Models
{
    public class LoginResponse
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
        public string? SessionKey { get; set; }
    }
}
