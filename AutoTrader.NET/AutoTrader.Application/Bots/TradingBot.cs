using AutoTrader.Application.Domains.Bot.Commands.SetBotStatus;
using AutoTrader.Application.Domains.Order.Commands;
using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Enums;
using AutoTrader.Domain.Helpers;
using Binance.Net.Interfaces.Clients;
using Binance.Spot;
using Binance.Spot.Models;
using MediatR;
using System.Net;
using TicTacTec.TA.Library;
using static TicTacTec.TA.Library.Core;
using Side = Binance.Spot.Models.Side;

namespace AutoTrader.Application.Bots
{
    internal class TradingBot
    {
        public BotSettings Settings { get; init; }

        private Task task = null!;
        private CancellationTokenSource tokenSource = new();
        private readonly IBinanceRestClient _binanceClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMediator _mediator;

        private const int oneDayMiliseconds = 86_400_000;

        private bool isOpenPosition = false;
        private double openPositionPrice;

        public TradingBot(BotSettings settings,
            IBinanceRestClient binanceRestClient,
            IHttpClientFactory httpClientFactory,
            IMediator mediator)
        {
            Settings = settings;
            _binanceClient = binanceRestClient;
            _httpClientFactory = httpClientFactory;
            _mediator = mediator;
        }

        public void Start()
        {
            tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            task = new Task(async () =>
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await _mediator.Send(new SetBotStatusCommand(Settings.BotId, Status.InProgress, StatusMessage.InProgress));
                    Settings.Status = Status.InProgress;

                    using HttpClient httpClient = _httpClientFactory.CreateClient();
                    var spotAccountTrade = new SpotAccountTrade(httpClient, apiKey: Settings.ApiKey, apiSecret: Settings.ApiSecret);
                    var prevMACDHist = 0.0;

                    bool moreToDo = true;
                    while (moreToDo)
                    {
                        var closePrices = (await GetClosePrices(Settings.Symbol, 1440, 60)).ToArray();
                        var macdHists = GetMACDHist(closePrices, Settings.FastLength, Settings.SlowLength, Settings.SignalLength);

                        /*if (isOpenPosition && closePrices.Last() * Settings.Quantity < openPositionPrice * Settings.Quantity * 0.8)
                        {
                            var createOrderCommand = new CreateOrderCommand
                            {
                                Symbol = Settings.Symbol,
                                Price = closePrices.Last(),
                                Side = Domain.Enums.Side.Sell,
                                Quantity = Settings.Quantity,
                                BotId = Settings.BotId
                            };

                            await _mediator.Send(createOrderCommand);
                            isOpenPosition = false;
                            continue;
                        }*/

                        if (prevMACDHist < 0 && macdHists.Last() > 0 && !isOpenPosition)
                        {
                            var response = await httpClient.GetAsync("http://127.0.0.1:8000/api/lstm/" + Settings.Symbol);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                // place buy order
                                /*var stopLossPrice = (decimal)(closePrices.Last() * 0.99);
                                var marketOrderRes = await spotAccountTrade.NewOrder(Settings.Symbol, Side.BUY, OrderType.MARKET, quantity: (decimal)Settings.Quantity);*/
                                var createOrderCommand = new CreateOrderCommand
                                {
                                    Symbol = Settings.Symbol,
                                    Price = closePrices.Last(),
                                    Side = Domain.Enums.Side.Buy,
                                    Quantity = Settings.Quantity,
                                    BotId = Settings.BotId
                                };

                                await _mediator.Send(createOrderCommand);
                                isOpenPosition = true;
                                openPositionPrice = closePrices.Last();
                            }
                        }

                        if (prevMACDHist > 0 && macdHists.Last() < 0 && isOpenPosition)
                        {
                            var response = await httpClient.GetAsync("http://127.0.0.1:8000/api/lstm/" + Settings.Symbol);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                // place sell order

                                var createOrderCommand = new CreateOrderCommand
                                {
                                    Symbol = Settings.Symbol,
                                    Price = closePrices.Last(),
                                    Side = Domain.Enums.Side.Sell,
                                    Quantity = Settings.Quantity,
                                    BotId = Settings.BotId
                                };

                                await _mediator.Send(createOrderCommand);
                                isOpenPosition = false;
                            }
                        }

                        prevMACDHist = macdHists.Last();

                        if (cancellationToken.IsCancellationRequested)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        await Task.Delay(60_000);
                    }
                }
                catch (OperationCanceledException)
                {
                    try
                    {
                        await _mediator.Send(new SetBotStatusCommand(Settings.BotId, Status.Paused, StatusMessage.PausedByUser));
                    } catch { }
                    Settings.Status = Status.Paused;
                }
                catch (Exception ex)
                {
                    try
                    {
                        await _mediator.Send(new SetBotStatusCommand(Settings.BotId, Status.Paused, StatusMessage.PausedByError(ex.InnerException.Message)));
                    } catch { }
                    Settings.Status = Status.Paused;
                }


            }, cancellationToken);
            task.Start();
        }

        public void Stop()
        {
            Settings.Status = Status.Paused;
            tokenSource.Cancel();
        }

        private async Task<IEnumerable<double>> GetClosePrices(string symbol, int intervarInMinutes, int periodInDays)
        {
            int m = intervarInMinutes % 60;
            int h = (intervarInMinutes - m) / 60;
            var klInterval = new TimeSpan(h, m, 0);

            var response = await _binanceClient.SpotApi.CommonSpotClient
                .GetKlinesAsync(symbol, klInterval,
                    DateTime.Now.AddDays(-1 * periodInDays), DateTime.Now);

            if (response.Success)
            {
                return response.Data.Select(k => Decimal.ToDouble(k.ClosePrice.GetValueOrDefault(0)));
            }

            return Enumerable.Empty<double>();
        }

        private double[] GetMACDHist(double[] closePrices, int optInFastPeriod, int optInSlowPeriod, int optInSignalPeriod) 
        {
            double[] outMACD = new double[closePrices.Length];
            double[] outMACDSignal = new double[closePrices.Length];
            double[] outMACDHist = new double[closePrices.Length];

            var retCode = Core.Macd(0, closePrices.Length - 1,
                closePrices, optInFastPeriod,
                optInSlowPeriod, optInSignalPeriod,
                out _, out int outNBElement,
                outMACD, outMACDSignal, outMACDHist);

            if (retCode == RetCode.Success)
            {
                var res = new double[outNBElement];
                for (int i = 0; i < outNBElement; i++)
                {
                    res[i] = outMACDHist[i];
                }
                return res;
            }

            return Array.Empty<double>();
        }
    }
}
