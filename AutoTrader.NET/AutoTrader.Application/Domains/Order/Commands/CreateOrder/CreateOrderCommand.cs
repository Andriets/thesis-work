using AutoTrader.Domain.Enums;
using MediatR;

namespace AutoTrader.Application.Domains.Order.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public string Symbol { get; set; } = null!;

        public double Price { get; set; }

        public Side Side { get; set; }

        public double Quantity { get; set; }

        public Guid BotId { get; set; }
    }
}
