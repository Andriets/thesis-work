using AutoTrader.Application.Dtos;
using AutoTrader.Domain.Abstractions;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Domains.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IAppDbContext _context;

        public GetUserByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(u => u.Id == request.Id).SingleOrDefault();

            if (user is null)
            {
                throw new AutoTraderException(ErrorHelper.EntityNotFound(nameof(AppUser)));
            }

            var res = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                ApiKey = user.ApiKey,
                ApiSecret = user.ApiSecret,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return Task.FromResult(res);
        }
    }
}
