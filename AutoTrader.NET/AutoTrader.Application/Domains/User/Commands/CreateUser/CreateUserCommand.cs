using MediatR;

namespace AutoTrader.Application.Domains.User.Commands.CreateUser
{
    public record CreateUserCommand(string UserName, string Email, string Password) : IRequest<Guid>;
}
