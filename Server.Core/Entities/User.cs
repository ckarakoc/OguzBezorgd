using Microsoft.AspNetCore.Identity;

namespace Server.Core.Entities;

/// <summary>
/// A User, based on AspNetCore IdentityUser&lt;int\&gt;.
/// Only a User with the Role of Customer can create zero or many Orders.
/// Only a User with the Role of Partner can create zero or many Stores.
/// A User can have zero or many user-specific addresses.
/// </summary>
public class User : IdentityUser<int>
{
    // User -> Has -> [Child]
    public List<Order> Orders { get; set; } = []; // todo: nullable? only a 'Customer' can make orders
    public List<Store> Stores { get; set; } = []; // todo: nullable?  only a 'Partner' can make stores
    public List<UserAddress> Addresses { get; set; } = [];
}