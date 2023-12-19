using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Abstractions;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Queries.GetBotsByUser
{
    public class GetBotsByUserQueryHandler : IRequestHandler<GetBotsByUserQuery, IEnumerable<MacdBotDto>>
    {
        private readonly IAppDbContext _context;

        public GetBotsByUserQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<MacdBotDto>> Handle(GetBotsByUserQuery request, CancellationToken cancellationToken)
        {
            var bots = _context.Bots.Where(b => b.UserId == request.UserId).AsEnumerable();

            var res = bots.Select(bot => new MacdBotDto
            {
                Id = bot.Id,
                Name = bot.Name,
                Symbol = bot.Symbol,
                Quantity = bot.Quantity,
                Status = bot.Status,
                Message = bot.Message,
                FastLength = bot.FastLength,
                SlowLength = bot.SlowLength,
                SignalLength = bot.SignalLength,
                UserId = bot.UserId
            });

            return Task.FromResult(res);
        }
    }
}
