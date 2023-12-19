using AutoTrader.Application.Domains.Token.Commands.GenerateAccessToken;
using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutoTrader.Application.Domains.User.Commands.AuthorizeUser
{
    public class AuthorizeUserCommandHandler : IRequestHandler<AuthorizeUserCommand, AuthenticateResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMediator _mediator;

        public AuthorizeUserCommandHandler(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
        }

        public async Task<AuthenticateResponse> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new AutoTraderException(ErrorHelper.EntityNotFound("User"));
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!result.Succeeded)
            {
                throw new AutoTraderException("Login failed");
            }

            var jwtToken = await _mediator.Send(new GenerateAccessTokenCommand(user.Email));

            return new AuthenticateResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                JwtToken = jwtToken
            };
        }
    }
}
