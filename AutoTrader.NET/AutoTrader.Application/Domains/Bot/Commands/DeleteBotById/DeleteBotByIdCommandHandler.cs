using AutoTrader.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTrader.Application.Domains.Bot.Commands.DeleteBotById
{
    public class DeleteBotByIdCommandHandler : IRequestHandler<DeleteBotByIdCommand, Guid>
    {
        private readonly IAppDbContext _context;

        public DeleteBotByIdCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(DeleteBotByIdCommand request, CancellationToken cancellationToken)
        {
            var bot = _context.Bots
                .Include(b => b.Orders)
                .Where(b => b.Id == request.Id)
                .SingleOrDefault();

            if (bot is null)
            {
                return request.Id;
            }

            _context.Bots.Remove(bot);
            await _context.SaveChangesAsync();

            return request.Id;
        }
    }
}
