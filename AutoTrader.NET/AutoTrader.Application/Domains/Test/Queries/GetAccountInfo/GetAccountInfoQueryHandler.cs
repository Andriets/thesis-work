using Binance.Common;
using Binance.Spot;
using MediatR;

namespace AutoTrader.Application.Domains.Test.Queries.GetAccountInfo
{
    public class GetAccountInfoQueryHandler : IRequestHandler<GetAccountInfoQuery, string>
    {
        private readonly IHttpClientFactory _httpClientFactory = null!;

        public GetAccountInfoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient();
            var apiKey = "nEHvC17BIMJTRM1RJcJjtny6V0vTiGvGjbDUIBfOzaTTvHbO8f3ZB8jkwzeajpkv";
            var apiSecret = "JrmzdUt7ylxrrREg0mUbC6ueV5mudvMX5vnA8qt7ngCmasHFu4z9mzY2L1prJK41";
            var spotAccountTradeHMAC = new SpotAccountTrade(httpClient, new BinanceHmac(apiSecret), apiKey: apiKey);
            var resultHMAC = await spotAccountTradeHMAC.AccountInformation();

            return resultHMAC;
        }
    }
}
