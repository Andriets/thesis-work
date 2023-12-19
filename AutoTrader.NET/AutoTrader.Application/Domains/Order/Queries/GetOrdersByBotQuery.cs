using AutoTrader.Application.Dtos;
using MediatR;

namespace AutoTrader.Application.Domains.Order.Queries
{
    public record GetOrdersByBotQuery(Guid BotId) : IRequest<IEnumerable<OrderDto>>;
}
