using AutoTrader.Application.Domains.Bot.Commands.DeleteBotById;
using AutoTrader.Application.Domains.Bot.Commands.SetBotStatus;
using AutoTrader.Application.Domains.Bot.Queries.GetBotById;
using AutoTrader.Application.Domains.User.Queries.GetUserById;
using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Enums;
using AutoTrader.Domain.Exceptions;
using Binance.Net.Interfaces.Clients;
using MediatR;

namespace AutoTrader.Application.Bots
{
    public class BotService : IBotService
    {
        private readonly IMediator _mediator;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IBinanceRestClient _binanceClient;

        private List<TradingBot> Bots { get; init; } = new List<TradingBot>();

        public BotService(IMediator mediator,
            IHttpClientFactory httpClientFactory,
            IBinanceRestClient binanceRestClient)
        {
            _mediator = mediator;
            _httpClientFactory = httpClientFactory;
            _binanceClient = binanceRestClient;
        }

        public async Task StartBot(Guid userId, Guid botId)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(userId));

            if (string.IsNullOrWhiteSpace(user.ApiKey))
            {
                throw new AutoTraderException(nameof(user.ApiKey));
            }

            if (string.IsNullOrWhiteSpace(user.ApiSecret))
            {
                throw new AutoTraderException(nameof(user.ApiSecret));
            }

            var bot = await _mediator.Send(new GetBotByIdQuery(botId));

            if (Bots.Any(b => b.Settings.BotId == bot.Id))
            {
                throw new AutoTraderException("Bot is already started");
            }

            var botSettings = new BotSettings
            {
                BotId = bot.Id,
                UserId = user.Id,
                Symbol = bot.Symbol,
                Quantity = bot.Quantity,
                Status = bot.Status,
                Message = bot.Message,
                FastLength = bot.FastLength,
                SlowLength = bot.SlowLength,
                SignalLength = bot.SignalLength,
                ApiKey = user.ApiKey,
                ApiSecret = user.ApiSecret
            };

            var traidingBot = new TradingBot(botSettings, _binanceClient, _httpClientFactory, _mediator);
            Bots.Add(traidingBot);
            traidingBot.Start();
        }

        public async Task StopBot(Guid id)
        {
            var tradingBot = Bots.SingleOrDefault(b => b.Settings.BotId == id);

            if (tradingBot is null)
            {
                throw new AutoTraderException("Bot is not started");
            }

            if (tradingBot.Settings.Status == Status.Paused)
            {
                throw new AutoTraderException("Bot is already paused");
            }

            tradingBot.Stop();
            await _mediator.Send(new SetBotStatusCommand(tradingBot.Settings.BotId, Status.InProgress, "Pausing..."));
        }

        public void ContinueBot(Guid id)
        {
            var tradingBot = Bots.SingleOrDefault(b => b.Settings.BotId == id);

            if (tradingBot is null)
            {
                throw new AutoTraderException("Bot is not started");
            }

            if (tradingBot.Settings.Status == Status.InProgress)
            {
                throw new AutoTraderException("Bot is already in progress");
            }

            tradingBot.Start();
        }

        public async Task DeleteBot(Guid id)
        {
            var tradingBot = Bots.SingleOrDefault(b => b.Settings.BotId == id);

            if (tradingBot is not null)
            {
                tradingBot.Stop();
                Bots.Remove(tradingBot);
            }

            await _mediator.Send(new DeleteBotByIdCommand(id));
        }
    }
}
