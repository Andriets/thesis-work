using AutoTrader.Application.Dtos;
using MediatR;

namespace AutoTrader.Application.Domains.User.Queries
{
    public record GetUserInfoByTokenQuery(string JwtToken) : IRequest<UserDto>;
}
