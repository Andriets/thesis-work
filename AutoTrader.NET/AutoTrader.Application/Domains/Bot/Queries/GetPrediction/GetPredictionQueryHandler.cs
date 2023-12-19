using MediatR;
using System.Net;
using System.Net.Http.Json;

namespace AutoTrader.Application.Domains.Bot.Queries.GetPrediction
{
    public class GetPredictionQueryHandler : IRequestHandler<GetPredictionQuery>
    {
        record Error(string Message);
        record Prediction(double min5, double min30, double min60);
        public async Task Handle(GetPredictionQuery request, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();

            using var response = await httpClient.GetAsync("http://127.0.0.1:8000/api/lstm/BTCUSDT", cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            {
                // получаем информацию об ошибке
                Error? error = await response.Content.ReadFromJsonAsync<Error>();
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(error?.Message);
            }
            else
            {
                Prediction? result = await response.Content.ReadFromJsonAsync<Prediction>(cancellationToken: cancellationToken);

                // если запрос завершился успешно, получаем объект Person
                throw new NotImplementedException();
            }
            
        }
    }
}
