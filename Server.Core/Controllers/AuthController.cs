using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Core.DTOs;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Controllers;

public class AuthController(UserManager<User> userManager, IMapper mapper) : BaseApiController
{
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var userName = loginDto.UserName;
        var password = loginDto.Password;

        //todo
        // response?
        return Ok();
    }

    /// <summary>
    /// Invalidates the user’s session token on the server-side when logging out.
    /// </summary>
    /// <param name="token"></param> 
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] string token)
    {
        //todo
        return Ok();
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        if (await UserExist(registerDto.UserName))
        {
            return BadRequest("Username already exist");
        }

        var user = mapper.Map<User>(registerDto);

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new {userId = user.Id}, user);
    }

    /// <summary>
    /// Refresh the authentication token when it is close to expiring.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] string token)
    {
        //todo
        return Ok();
    }

    private async Task<bool> UserExist(string username)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
    }
}