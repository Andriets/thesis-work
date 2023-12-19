namespace AutoTrader.Application.Dtos
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string JwtToken { get; set; } = default!;
    }
}
