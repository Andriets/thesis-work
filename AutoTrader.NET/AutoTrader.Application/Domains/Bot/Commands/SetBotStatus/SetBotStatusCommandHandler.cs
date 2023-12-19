using AutoTrader.Domain.Abstractions;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTrader.Application.Domains.Bot.Commands.SetBotStatus
{
    public class SetBotStatusCommandHandler : IRequestHandler<SetBotStatusCommand>
    {
        private readonly IAppDbContext _context;

        public SetBotStatusCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(SetBotStatusCommand request, CancellationToken cancellationToken)
        {
            var bot = await _context.Bots.SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (bot is null)
            {
                throw new AutoTraderException(ErrorHelper.EntityNotFound(nameof(MacdBot)));
            }

            bot.Status = request.Status;
            bot.Message = request.Message;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
