using MediatR;

namespace AutoTrader.Application.Domains.Bot.Queries.GetBotProfit
{
    public record GetBotProfitQuery(Guid Id) : IRequest<double>;
}
