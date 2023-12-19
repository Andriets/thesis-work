using AutoTrader.Application.Dtos;
using MediatR;

namespace AutoTrader.Application.Domains.User.Commands.AuthorizeUser
{
    public record AuthorizeUserCommand(string Email, string Password) : IRequest<AuthenticateResponse>;
}
