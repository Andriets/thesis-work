using AutoTrader.Application.Dtos;
using MediatR;

namespace AutoTrader.Application.Domains.User.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
}
