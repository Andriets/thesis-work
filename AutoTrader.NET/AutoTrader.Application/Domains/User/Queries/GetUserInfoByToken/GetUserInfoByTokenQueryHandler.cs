using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AutoTrader.Application.Domains.User.Queries
{
    public class GetUserInfoByTokenQueryHandler : IRequestHandler<GetUserInfoByTokenQuery, UserDto>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserInfoByTokenQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(GetUserInfoByTokenQuery request, CancellationToken cancellationToken)
        {
            var stream = request.JwtToken;
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadToken(stream);
            var jwtToken = securityToken as JwtSecurityToken;
            var email = jwtToken!.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new AutoTraderException(ErrorHelper.EntityNotFound(nameof(AppUser)));
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                ApiKey = user.ApiKey,
                ApiSecret = user.ApiSecret,
                Email = email,
                PhoneNumber = user.PhoneNumber
            };
            return userDto;
        }
    }
}
