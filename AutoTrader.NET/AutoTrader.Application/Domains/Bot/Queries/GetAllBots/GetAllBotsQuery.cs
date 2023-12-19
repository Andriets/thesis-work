using AutoTrader.Application.Dtos;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Queries.GetAllBots
{
    public record GetAllBotsQuery : IRequest<IEnumerable<MacdBotDto>>;
}
