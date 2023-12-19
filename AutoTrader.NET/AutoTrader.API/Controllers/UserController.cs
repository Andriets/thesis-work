using AutoTrader.Application.Domains.Test.Commants.CreateSpotOrder;
using AutoTrader.Application.Domains.Test.Queries.GetAccountInfo;
using AutoTrader.Application.Domains.User.Commands.AuthorizeUser;
using AutoTrader.Application.Domains.User.Commands.CreateUser;
using AutoTrader.Application.Domains.User.Commands.SetUserApiKey;
using AutoTrader.Application.Domains.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrader.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp(CreateUserCommand request)
        {
            var res = await _mediator.Send(request);
            return Created("", new { UserId = res });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn(AuthorizeUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> WhoAmIAsync()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth);
            var jwtToken = headerAuth.First()!.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
            var query = new GetUserInfoByTokenQuery(jwtToken);
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAccountInfo()
        {
            var res = await _mediator.Send(new GetAccountInfoQuery());
            return Ok(res);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> SetUserApiKey([FromBody] SetUserApiKeyCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> MakeSpotOrder(CreateSpotOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /*[HttpPost("[action]")]
        public async Task<IActionResult> MakeSpotOrder(CreateSpotOrderCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(res);
        }*/
    }
}
