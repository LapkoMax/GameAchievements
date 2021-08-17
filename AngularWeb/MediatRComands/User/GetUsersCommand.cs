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
    public class GetUsersCommand : IRequest<IEnumerable<UserDto>>
    {

    }
    public class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, IEnumerable<UserDto>>
    {
        private readonly UserManager<Entities.Models.User> _userManager;
        public GetUsersCommandHandler(UserManager<Entities.Models.User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<UserDto>> Handle(GetUsersCommand command, CancellationToken token)
        {
            var users = _userManager.Users;
            var usersToReturn = new List<UserDto>();
            foreach (Entities.Models.User user in users)
            {
                usersToReturn.Add(new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
            return usersToReturn;
        }
    }
}
