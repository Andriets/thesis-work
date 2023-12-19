using AutoTrader.Domain.Enums;

namespace AutoTrader.Domain.Entities
{
    public class MacdBot : BaseEntity<Guid>
    {
        public string Name { get; set; } = null!;

        public string Symbol { get; set; } = null!;

        public double Quantity { get; set; }

        public Status Status { get; set; }

        public string? Message { get; set; }

        public int FastLength { get; set; }

        public int SlowLength { get; set; }

        public int SignalLength { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
