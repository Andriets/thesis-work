using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Abstractions;
using MediatR;

namespace AutoTrader.Application.Domains.Order.Queries
{
    public class GetOrdersByBotQueryHandler : IRequestHandler<GetOrdersByBotQuery, IEnumerable<OrderDto>>
    {
        private readonly IAppDbContext _context;

        public GetOrdersByBotQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<OrderDto>> Handle(GetOrdersByBotQuery request, CancellationToken cancellationToken)
        {
            var orders = _context.Orders.Where(o => o.BotId == request.BotId).AsEnumerable();

            var res = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                Date = order.Date,
                Symbol = order.Symbol,
                Price = order.Price,
                Side = order.Side,
                Quantity = order.Quantity,
                BotId = order.BotId
            });

            return Task.FromResult(res);
        }
    }
}
