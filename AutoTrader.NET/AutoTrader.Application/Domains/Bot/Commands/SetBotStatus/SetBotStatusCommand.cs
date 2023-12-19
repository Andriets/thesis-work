using AutoTrader.Domain.Enums;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Commands.SetBotStatus
{
    public record SetBotStatusCommand(Guid Id, Status Status, string Message) : IRequest;
}
