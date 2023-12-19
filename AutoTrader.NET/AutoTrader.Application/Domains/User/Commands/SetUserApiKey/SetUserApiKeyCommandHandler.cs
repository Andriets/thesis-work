using AutoTrader.Domain.Abstractions;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTrader.Application.Domains.User.Commands.SetUserApiKey
{
    public class SetUserApiKeyCommandHandler : IRequestHandler<SetUserApiKeyCommand>
    {
        private readonly IAppDbContext _context;

        public SetUserApiKeyCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(SetUserApiKeyCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ApiKey))
            {
                throw new AutoTraderException(nameof(request.ApiKey));
            }

            if (string.IsNullOrWhiteSpace(request.ApiSecret))
            {
                throw new AutoTraderException(nameof(request.ApiSecret));
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user is null)
            {
                throw new AutoTraderException(ErrorHelper.EntityNotFound(nameof(AppUser)));
            }

            user.ApiKey = request.ApiKey;
            user.ApiSecret = request.ApiSecret;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
