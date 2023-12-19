using AutoTrader.Domain.Abstractions;
using MediatR;
using OrderEntity = AutoTrader.Domain.Entities.Order;

namespace AutoTrader.Application.Domains.Order.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IAppDbContext _context;

        public CreateOrderCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderEntity
            {
                Date = DateTime.Now,
                Symbol = request.Symbol,
                Price = request.Price,
                Side = request.Side,
                Quantity = request.Quantity,
                BotId = request.BotId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
