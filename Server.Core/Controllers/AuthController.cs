using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Interfaces;

namespace Server.Controllers;

public class AuthController(IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var username = loginDto.Username;
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
    public async Task<IActionResult> Logout([FromBody]string token)
    {
        //todo
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        //todo
        return Ok();
    }

    /// <summary>
    /// Refresh the authentication token when it is close to expiring.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody]string token)
    {
        //todo
        return Ok();
    }

}
