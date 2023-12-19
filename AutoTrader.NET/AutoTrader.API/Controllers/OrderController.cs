using AutoTrader.Application.Domains.Order.Commands;
using AutoTrader.Application.Domains.Order.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrader.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var res = await _mediator.Send(command);
            return Created("", new { id = res });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrdersByBot([FromQuery] GetOrdersByBotQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
    }
}
