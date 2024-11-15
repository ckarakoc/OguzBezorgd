using Microsoft.EntityFrameworkCore;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Data.Repositories;

public class StoreRepository(DataContext context) : IStoreRepository
{
    public void AddStore(Store store)
    {
        context.Stores.Add(store);
    }

    public async Task<IEnumerable<Store>> GetStoresAsync()
    {
        var stores = await context.Stores
            .Include(s => s.Address)
            .ToListAsync();
        return stores;
    }

    public async Task<Store?> GetStoreByIdAsync(int id)
    {
        var store = await context.Stores.FindAsync(id);
        return store;
    }

    public async Task<Store?> GetStoreByStoreNameAsync(string StoreName)
    {
        var store = await context.Stores.SingleOrDefaultAsync(store => store.StoreName == StoreName);
        return store;
    }

    public void UpdateStore(Store store)
    {
        context.Entry(store).State = EntityState.Modified;
    }

    public void DeleteStore(Store store)
    {
        context.Stores.Remove(store);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}