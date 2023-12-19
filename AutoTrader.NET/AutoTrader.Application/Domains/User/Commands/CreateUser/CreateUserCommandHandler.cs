using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutoTrader.Application.Domains.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly UserManager<AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new StrangeException("Failed to create a user");
            }

            return user.Id;
        }
    }
}
