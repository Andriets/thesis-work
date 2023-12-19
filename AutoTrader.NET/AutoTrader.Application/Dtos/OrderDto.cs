using AutoTrader.Domain.Enums;

namespace AutoTrader.Application.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Symbol { get; set; } = null!;

        public double Price { get; set; }

        public Side Side { get; set; }

        public double Quantity { get; set; }

        public Guid BotId { get; set; }
    }
}
