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
    public class UpdateUserCommand : IRequest<IdentityResult>
    {
        public string userId {get; set;}
        public UserDto user { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IdentityResult>
    {
        private readonly UserManager<Entities.Models.User> _userManager;
        public UpdateUserCommandHandler(UserManager<Entities.Models.User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> Handle(UpdateUserCommand command, CancellationToken token)
        {
            var userEntity = await _userManager.FindByIdAsync(command.userId);
            userEntity.FirstName = command.user.FirstName;
            userEntity.LastName = command.user.LastName;
            userEntity.UserName = command.user.UserName;
            userEntity.Email = command.user.Email;
            userEntity.PhoneNumber = command.user.PhoneNumber;
            var result = await _userManager.UpdateAsync(userEntity);
            return result;
        }
    }
}
