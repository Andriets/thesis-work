namespace AutoTrader.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string? ApiKey { get; set; }

        public string? ApiSecret { get; set; }

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }
    }
}
