using AutoTrader.Domain.Abstractions;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Enums;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Commands.CreateBot
{
    public class CreateBotCommandHandler : IRequestHandler<CreateBotCommand, Guid>
    {
        private readonly IAppDbContext _context;

        public CreateBotCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateBotCommand request, CancellationToken cancellationToken)
        {
            var bots = _context.Bots.Where(b => b.UserId == request.UserId);

            if (bots.Count() == 2)
            {
                throw new AutoTraderException("Only two bots are available in the same time");
            }

            var bot = new MacdBot
            {
                Name = request.Name,
                Symbol = request.Symbol,
                Quantity = request.Quantity,
                Status = Status.Created,
                Message = StatusMessage.Created,
                FastLength = request.FastLength,
                SlowLength = request.SlowLength,
                SignalLength = request.SignalLength,
                UserId = request.UserId
            };

            _context.Bots.Add(bot);
            await _context.SaveChangesAsync(cancellationToken);

            return bot.Id;
        }
    }
}
