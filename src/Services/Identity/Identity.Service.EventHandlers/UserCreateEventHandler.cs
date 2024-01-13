﻿using Identity.Domain;
using Identity.Service.EventHandlers.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Service.EventHandlers
{
    public class UserCreateEventHandler : IRequestHandler<UserCreateCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCreateEventHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;   
        }

        public async Task<IdentityResult> Handle(UserCreateCommand command, CancellationToken cancellationToken)
        {
            var entry = new ApplicationUser
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                UserName = command.Email
            };

            return await _userManager.CreateAsync(entry, command.Password);
        }

    }
}