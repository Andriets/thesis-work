using MediatR;

namespace AutoTrader.Application.Domains.Token.Commands.GenerateAccessToken
{
    public record GenerateAccessTokenCommand(string Email) : IRequest<string>;
}
