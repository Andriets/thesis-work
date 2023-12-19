using AutoTrader.Application.Dtos;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Queries.GetBotById
{
    public record GetBotByIdQuery(Guid Id) : IRequest<MacdBotDto>;
}
