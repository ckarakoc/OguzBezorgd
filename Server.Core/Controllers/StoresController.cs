using Microsoft.AspNetCore.Mvc;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Controllers;

public class StoresController(
    IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpGet] // api/stores
    public async Task<ActionResult<IEnumerable<Store>>> GetStores()
    {
        var stores = await unitOfWork.StoreRepository.GetStoresAsync();
        return Ok(stores);
    }
}