using Server.Entities;

namespace Server.Interfaces;

public interface IStoreRepository
{
    // Create Operators
    void AddStore(Store store);
    
    // Read Operations
    Task<IEnumerable<Store>> GetStoresAsync();
    Task<Store?> GetStoreByIdAsync(int id);
    Task<Store?> GetStoreByStoreNameAsync(string StoreName);
    
    // Update operations
    void UpdateStore(Store store);
    
    // Delete Operations
    void DeleteStore(Store store);
    
    Task Save();
}