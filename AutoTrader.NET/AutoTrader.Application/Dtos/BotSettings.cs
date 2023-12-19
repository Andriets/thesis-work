using AutoTrader.Domain.Enums;

namespace AutoTrader.Application.Dtos
{
    internal class BotSettings
    {
        public Guid BotId { get; set; }

        public Guid UserId { get; set; }

        public string Symbol { get; set; } = null!;

        public double Quantity { get; set; }

        public Status Status { get; set; }

        public string? Message { get; set; }

        public int FastLength { get; set; }

        public int SlowLength { get; set; }

        public int SignalLength { get; set; }

        public string ApiKey { get; set; } = null!;

        public string ApiSecret { get; set; } = null!;
    }
}
