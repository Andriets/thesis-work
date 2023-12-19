using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Abstractions;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Queries.GetAllBots
{
    public class GetAllBotsQueryHandler : IRequestHandler<GetAllBotsQuery, IEnumerable<MacdBotDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllBotsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<MacdBotDto>> Handle(GetAllBotsQuery request, CancellationToken cancellationToken)
        {
            var bots = _context.Bots.AsEnumerable();

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
