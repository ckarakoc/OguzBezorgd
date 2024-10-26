using Microsoft.AspNetCore.Identity;

namespace Server.Entities;

public class User : IdentityUser<int>
{
    // User -> Has -> [Child]
    public List<Order> Orders { get; set; } = []; // todo: nullable? only a 'Customer' can make orders
    public List<Store> Store { get; set; } = []; // todo: nullable?  only a 'Partner' can make stores
    public List<UserAddress> Addresses { get; set; } = [];
}