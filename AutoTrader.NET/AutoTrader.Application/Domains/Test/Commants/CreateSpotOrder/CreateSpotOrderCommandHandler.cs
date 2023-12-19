using AutoTrader.Application.Domains.User.Queries.GetUserById;
using Binance.Spot;
using Binance.Spot.Models;
using MediatR;

namespace AutoTrader.Application.Domains.Test.Commants.CreateSpotOrder
{
    public class CreateSpotOrderCommandHandler : IRequestHandler<CreateSpotOrderCommand, string>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMediator _mediator;

        public CreateSpotOrderCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            _httpClientFactory = httpClientFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateSpotOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(request.UserId));

            using HttpClient httpClient = _httpClientFactory.CreateClient();
            var spotAccountTrade = new SpotAccountTrade(httpClient, apiKey: user.ApiKey, apiSecret: user.ApiSecret);

            var side = request.Side == Domain.Enums.Side.Buy ? Side.BUY : Side.SELL; 

            var marketOrderRes = await spotAccountTrade.NewOrder(request.Symbol, side, OrderType.MARKET, quantity: request.Quantity);

            return marketOrderRes;
        }
    }
}
