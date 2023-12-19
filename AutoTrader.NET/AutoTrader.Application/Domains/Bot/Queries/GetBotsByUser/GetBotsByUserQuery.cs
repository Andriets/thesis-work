using AutoTrader.Application.Dtos;
using MediatR;

namespace AutoTrader.Application.Domains.Bot.Queries.GetBotsByUser
{
    public record GetBotsByUserQuery(Guid UserId) : IRequest<IEnumerable<MacdBotDto>>;
}
