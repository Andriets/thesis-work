using AutoTrader.Application.Dtos;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoTrader.Application.Domains.Token.Commands.GenerateAccessToken
{
    public class GenerateAccessTokenCommandHandler : IRequestHandler<GenerateAccessTokenCommand, string>
    {
        private readonly IOptions<JwtOptions> _jwtOptions;

        public GenerateAccessTokenCommandHandler(IOptions<JwtOptions> opt)
        {
            _jwtOptions = opt;
        }

        public Task<string> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var lifeTime = _jwtOptions.Value.LifeTime;
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, request.Email),
            };

            var secretBytes = Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(lifeTime),
                signingCredentials: new SigningCredentials(key, algorithm)
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }
    }
}
