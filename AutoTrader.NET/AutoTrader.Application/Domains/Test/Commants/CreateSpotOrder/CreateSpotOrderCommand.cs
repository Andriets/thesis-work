using AutoTrader.Domain.Enums;
using MediatR;

namespace AutoTrader.Application.Domains.Test.Commants.CreateSpotOrder
{
    public record CreateSpotOrderCommand(Guid UserId, string Symbol, Side Side, decimal Quantity ) : IRequest<string>;
}
