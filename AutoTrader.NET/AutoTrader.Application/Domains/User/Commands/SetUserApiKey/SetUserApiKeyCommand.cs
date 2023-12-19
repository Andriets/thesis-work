using MediatR;

namespace AutoTrader.Application.Domains.User.Commands.SetUserApiKey
{
    public record SetUserApiKeyCommand(Guid Id, string ApiKey, string ApiSecret) : IRequest;
}
