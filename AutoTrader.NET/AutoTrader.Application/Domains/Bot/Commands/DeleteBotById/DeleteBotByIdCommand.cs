using MediatR;

namespace AutoTrader.Application.Domains.Bot.Commands.DeleteBotById
{
    public record DeleteBotByIdCommand(Guid Id) : IRequest<Guid>;
}
