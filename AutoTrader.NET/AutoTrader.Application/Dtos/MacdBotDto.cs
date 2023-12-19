using AutoTrader.Domain.Enums;

namespace AutoTrader.Application.Dtos
{
    public class MacdBotDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Symbol { get; set; } = null!;

        public double Quantity { get; set; }

        public Status Status { get; set; }

        public string Message { get; set; } = null!;

        public int FastLength { get; set; }

        public int SlowLength { get; set; }

        public int SignalLength { get; set; }

        public Guid UserId { get; set; }
    }
}
