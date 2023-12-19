using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Abstractions;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTrader.Application.Domains.Bot.Queries.GetBotById
{
    public class GetBotByIdQueryHandler : IRequestHandler<GetBotByIdQuery, MacdBotDto>
    {
        private readonly IAppDbContext _context;

        public GetBotByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<MacdBotDto> Handle(GetBotByIdQuery request, CancellationToken cancellationToken)
        {
            var bot = await _context.Bots.SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (bot is null)
            {
                throw new AutoTraderException(ErrorHelper.EntityNotFound(nameof(MacdBot)));
            }

            var res = new MacdBotDto
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
            };

            return res;
        }
    }
}
