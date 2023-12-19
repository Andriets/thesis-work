using Microsoft.AspNetCore.Identity;

namespace AutoTrader.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? ApiKey { get; set; }

        public string? ApiSecret { get; set; }

        public ICollection<MacdBot> Bots { get; set; } = new List<MacdBot>();
    }
}
