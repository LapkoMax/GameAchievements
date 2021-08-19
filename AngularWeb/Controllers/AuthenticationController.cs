using AngularWeb.MediatRComands.User;
using Entities.Authentication;
using Entities.DataTransferObjects;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationManager _authManager;
        private readonly IMediator _mediator;
        public IConfiguration Configuration { get; }
        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IAuthenticationManager authManager, IConfiguration configuration, IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authManager = authManager;
            Configuration = configuration;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto
            userForRegistration)
        {
            var result = await _mediator.Send(new AddNewUserCommand { user = userForRegistration }, CancellationToken.None);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authManager.ValidateUser(user))
            {
                return Unauthorized("Authentication failed. Wrong username or password.");
            }
            var token = await _authManager.CreateToken();
            return Ok(new { Token = token });
        }

        [HttpGet("users"), Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var usersToReturn = await _mediator.Send(new GetUsersCommand(), CancellationToken.None);
            return Ok(usersToReturn);
        }

        [HttpPut("user/{id}"), Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody]UserDto user)
        {
            var result = await _mediator.Send(new UpdateUserCommand { userId = id, user = user }, CancellationToken.None);
            return Ok(result.Errors);
        }

        [HttpDelete("user/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return Ok("Success");
        }

        [HttpGet("roles"), Authorize]
        public async Task<IActionResult> GetUserRoles([FromQuery] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles= await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpGet("allRoles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(_roleManager.Roles.ToList());
        }

        [HttpPost("allRoles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return Ok();
        }

        [HttpPut("allRoles/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(string id, IdentityRole role)
        {
            var roleEntity = await _roleManager.FindByIdAsync(id);
            roleEntity.Name = role.Name;
            await _roleManager.UpdateAsync(roleEntity);
            return Ok();
        }

        [HttpPut("user/{userName}/roles/{roleIds}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRoles([FromRoute]string userName, [FromRoute] string roleIds)
        {
            var result = await _mediator.Send(new UpdateUserRolesCommand { userName = userName, roleIds = roleIds }, CancellationToken.None);
            return Ok(result);
        }

        [HttpDelete("allRoles/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return Ok();
        }
    }
}
