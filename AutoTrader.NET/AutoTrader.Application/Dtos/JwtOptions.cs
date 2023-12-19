namespace AutoTrader.Application.Dtos
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = default!;

        public double LifeTime { get; set; }
    }
}
