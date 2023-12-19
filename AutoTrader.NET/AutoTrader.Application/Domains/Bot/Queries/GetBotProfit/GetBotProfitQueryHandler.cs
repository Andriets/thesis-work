using AutoTrader.Application.Domains.Order.Queries;
using AutoTrader.Domain.Abstractions;
using AutoTrader.Domain.Enums;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Queries.GetBotProfit
{
    public class GetBotProfitQueryHandler : IRequestHandler<GetBotProfitQuery, double>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;

        public GetBotProfitQueryHandler(IAppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<double> Handle(GetBotProfitQuery request, CancellationToken cancellationToken)
        {
            var botOrders = await _mediator.Send(new GetOrdersByBotQuery(request.Id), cancellationToken);

            if (!botOrders.Any())
            {
                return 0;
            }

            double profit = 0;

            foreach (var botOrder in botOrders)
            {
                profit = botOrder.Side == Side.Buy 
                    ? profit - botOrder.Price * botOrder.Quantity
                    : profit + botOrder.Price * botOrder.Quantity;
            }

            return profit;
        }
    }
}
