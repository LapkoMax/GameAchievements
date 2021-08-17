using AutoMapper;
using Entities.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.User
{
    public class AddNewUserCommand : IRequest<IdentityResult>
    {
        public UserForRegistrationDto user { get; set; }
    }
    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, IdentityResult>
    {
        private readonly UserManager<Entities.Models.User> _userManager;
        private readonly IMapper _mapper;
        public AddNewUserCommandHandler(UserManager<Entities.Models.User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IdentityResult> Handle(AddNewUserCommand command, CancellationToken token)
        {
            var user = _mapper.Map<Entities.Models.User>(command.user);
            var result = await _userManager.CreateAsync(user, command.user.Password);
            if (!result.Succeeded)
            {
                return result;
            }
            await _userManager.AddToRolesAsync(user, command.user.Roles);
            return result;
        }
    }
}
