using MediatR;

namespace AutoTrader.Application.Domains.Bot.Commands.CreateBot
{
    public class CreateBotCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;

        public string Symbol { get; set; } = null!;

        public double Quantity { get; set; }

        public int FastLength { get; set; } = 12;

        public int SlowLength { get; set; } = 26;

        public int SignalLength { get; set; } = 9;

        public Guid UserId { get; set; }
    }
}
