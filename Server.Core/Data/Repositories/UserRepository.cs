using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.Core.DTOs;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Data.Repositories;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public void AddUser(User user)
    {
        context.Users.Add(user);
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        return users;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        return user;
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var user = await context.Users.Include(u => u.Stores).SingleOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task SaveRefreshToken(User user, string refreshToken, DateTime expiryDate)
    {
        var existingToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == user.Id);

        if (existingToken != null)
        {
            // Update the existing token
            existingToken.Token = refreshToken;
            existingToken.ExpiryDate = expiryDate;
        }
        else
        {
            var token = new RefreshToken
            {
                User = user,
                Token = refreshToken,
                ExpiryDate = expiryDate
            };
            context.RefreshTokens.Add(token);
        }

        await SaveAsync();
    }

    public async Task<RefreshToken?> GetRefreshToken(User user)
    {
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == user.Id);
        return token;
    }
    
    public async Task DeleteRefreshToken(User user)
    {
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == user.Id);

        if (token != null)
        {
            context.RefreshTokens.Remove(token);
            await SaveAsync();
        }
    }


    public void UpdateUser(User user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public void DeleteUser(User user)
    {
        context.Users.Remove(user);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

}