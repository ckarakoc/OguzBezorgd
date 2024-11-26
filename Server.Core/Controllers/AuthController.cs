using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Server.Core.DTOs;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Controllers;

public class AuthController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<IdentityRole<int>> roleManager,
    ITokenService tokenService,
    IMapper mapper) : BaseApiController
{
    /// <summary>
    /// Handles user login by validating the username and password.
    /// </summary>
    /// <param name="loginDto">An object containing the user's login details (username and password).</param>
    /// <returns>
    /// Returns an HTTP response with:
    /// - 200 OK: If login is successful, includes user details and a generated authentication token.
    /// - 400 Bad Request: If no user is found with the provided username.
    /// - 401 Unauthorized: If the password is incorrect.
    /// </returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        //todo: write tests
        var userName = loginDto.UserName;
        var password = loginDto.Password;

        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return BadRequest("No user found");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(result);
        }

        signInManager.IsSignedIn(User);

        Log.Information("User {@user} logged in", user);

        var accessToken = await tokenService.GenerateTokenAsync(user, DateTime.UtcNow.AddMinutes(15));
        var refreshToken = await tokenService.GenerateTokenAsync(user, DateTime.UtcNow.AddDays(90));

        await userManager.SetAuthenticationTokenAsync(user, "OguzBezorgd", "RefreshToken", refreshToken);

        return Ok(new LoginResponseDto
        {
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    /// <summary>
    /// Invalidates the user’s session token on the server-side when logging out.
    /// </summary>
    /// <param name="token"></param> 
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(AuthUserDto authUserDto)
    {
        var user = await userManager.FindByNameAsync(authUserDto.UserName);
        if (user == null)
        {
            return BadRequest("No user found");
        }

        await userManager.RemoveAuthenticationTokenAsync(user, "OguzBezorgd", "RefreshToken");

        return Ok("Logged out");
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

        if (!await roleManager.RoleExistsAsync(registerDto.Role))
        {
            return BadRequest("Role does not exist");
        }

        var user = mapper.Map<User>(registerDto);
        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        Log.Information("User {@user} registered", user);
        
        await userManager.AddToRoleAsync(user, registerDto.Role);
        Log.Information("Roles of user: {@roles} ", await userManager.GetRolesAsync(user));

        return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new {userId = user.Id}, user);
    }

    /// <summary>
    /// Refresh the authentication token when it is close to expiring.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh(AuthUserDto userDto, string token)
    {
        var user = await userManager.FindByNameAsync(userDto.UserName);
        if (user == null)
        {
            return BadRequest("No user found");
        }

        // check refresh token is in the DB
        var storedRefreshToken = await userManager.GetAuthenticationTokenAsync(user, "OguzBezorgd", "RefreshToken");
        if (storedRefreshToken == null || storedRefreshToken != token)
        {
            return Unauthorized("Invalid refresh token.");
        }

        // check if the token is verified
        var valid = await tokenService.CheckTokenIsValid(token);
        if (!valid)
        {
            return BadRequest("Invalid token");
        }

        // generate new access token
        var accessToken = await tokenService.GenerateTokenAsync(user, DateTime.UtcNow.AddMinutes(15));
        return Ok(new {accessToken});
    }

    private async Task<bool> UserExist(string username)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
    }
}