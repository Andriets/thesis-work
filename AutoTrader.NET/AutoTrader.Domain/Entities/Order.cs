using AutoTrader.Domain.Enums;

namespace AutoTrader.Domain.Entities
{
    public class Order : BaseEntity<Guid>
    {
        public DateTime Date { get; set; }

        public string Symbol { get; set; } = null!;

        public double Price { get; set; }

        public Side Side { get; set; }

        public double Quantity { get; set; }

        public Guid BotId { get; set; }

        public MacdBot Bot { get; set; } = null!;
    }
}
