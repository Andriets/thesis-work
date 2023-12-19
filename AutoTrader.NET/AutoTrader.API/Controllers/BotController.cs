using AutoTrader.Application.Bots;
using AutoTrader.Application.Domains.Bot.Commands.CreateBot;
using AutoTrader.Application.Domains.Bot.Commands.DeleteBotById;
using AutoTrader.Application.Domains.Bot.Queries.GetAllBots;
using AutoTrader.Application.Domains.Bot.Queries.GetBotProfit;
using AutoTrader.Application.Domains.Bot.Queries.GetBotsByUser;
using AutoTrader.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrader.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IBotService _botService;

        public BotController(IMediator mediator, IBotService botService)
        {
            _mediator = mediator;
            _botService = botService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateBot([FromBody] CreateBotCommand command)
        {
            var res = await _mediator.Send(command);
            return Created("", new { id = res });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllBots()
        {
            var res = await _mediator.Send(new GetAllBotsQuery());
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBotsByUser([FromQuery] GetBotsByUserQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBotProfit([FromQuery] GetBotProfitQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> StartBot([FromBody] StartBotCommand command)
        {
            await _botService.StartBot(command.userId, command.botId);
            return Ok();
        }

        [HttpPatch("[action]")]
        public IActionResult PauseBot([FromBody] PauseBotCommand command)
        {
            _botService.StopBot(command.Id);
            return Ok();
        }

        [HttpPatch("[action]")]
        public IActionResult ContinueBot([FromBody] ContinueBotCommand command)
        {
            _botService.ContinueBot(command.Id);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteBotById([FromBody] DeleteBotByIdCommand command)
        {
            await _botService.DeleteBot(command.Id);
            return Ok();
        }
    }
}
