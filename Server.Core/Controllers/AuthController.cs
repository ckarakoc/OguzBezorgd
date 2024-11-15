// using AutoMapper;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Serilog;
// using Server.Core.DTOs;
// using Server.Core.Entities;
// using Server.Core.Interfaces;

// namespace Server.Core.Controllers;

// public class AuthController(
//     UserManager<User> userManager,
//     SignInManager<User> signInManager,
//     RoleManager<IdentityRole<int>> roleManager,
//     IMapper mapper) : BaseApiController
// {
//     [HttpPost("login")]
//     public async Task<ActionResult> Login(LoginDto loginDto)
//     {
//         var userName = loginDto.UserName;
//         var password = loginDto.Password;

//         var user = await userManager.FindByNameAsync(userName);
//         if (user == null)
//         {
//             return BadRequest("No user found");
//         }

//         var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
//         if (!result.Succeeded)
//         {
//             return Unauthorized(result);
//         }

//         var responseDto = mapper.Map<LoginResponseDto>(user);
//         Log.Information("User {@user} logged in", user);

//         return Ok(responseDto);
//     }

//     /// <summary>
//     /// Invalidates the user’s session token on the server-side when logging out.
//     /// </summary>
//     /// <param name="token"></param> 
//     /// <returns></returns>
//     [HttpPost("logout")]
//     public async Task<ActionResult> Logout([FromBody] string token)
//     {
//         await signInManager.SignOutAsync();
//         return Ok("Logged out");
//     }

//     /// <summary>
//     /// Creates a new user
//     /// </summary>
//     /// <param name="registerDto"></param>
//     /// <returns></returns>
//     [HttpPost("register")]
//     public async Task<ActionResult> Register(RegisterDto registerDto)
//     {
//         if (await UserExist(registerDto.UserName))
//         {
//             return BadRequest("Username already exist");
//         }

//         if (!await roleManager.RoleExistsAsync(registerDto.Role))
//         {
//             return BadRequest("Role does not exist");
//         }

//         var user = mapper.Map<User>(registerDto);
//         var result = await userManager.CreateAsync(user, registerDto.Password);
//         Log.Information("User {@user} registered", user);

//         await userManager.AddToRoleAsync(user, registerDto.Role);
//         Log.Information("Roles of user: {@roles} ", await userManager.GetRolesAsync(user));
//         if (!result.Succeeded)
//         {
//             return BadRequest(result.Errors);
//         }

//         return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new {userId = user.Id}, user);
//     }

//     /// <summary>
//     /// Refresh the authentication token when it is close to expiring.
//     /// </summary>
//     /// <param name="token"></param>
//     /// <returns></returns>
//     [HttpPost("refresh")]
//     public async Task<ActionResult> Refresh([FromBody] string token)
//     {
//         //todo
//         return Ok();
//     }

//     private async Task<bool> UserExist(string username)
//     {
//         return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
//     }
// }