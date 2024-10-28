using Server.Entities;

namespace Server.Interfaces;

public interface IUserRepository
{
    // Create Operators
    void AddUser(User user);
    
    // Read Operations
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUserNameAsync(string userName);
    Task<User?> GetUserByEmailAsync(string email);
    
    // Update operations
    void UpdateUser(User user);
    
    // Delete Operations
    void DeleteUser(User user);
    
    void Save();
}