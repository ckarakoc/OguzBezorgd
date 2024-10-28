using Microsoft.AspNetCore.Mvc;
using Server.Entities;
using Server.Interfaces;

namespace Server.Controllers;

public class StoresController(
    IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Store>>> GetStores()
    {
        var stores = await unitOfWork.StoreRepository.GetStoresAsync();
        return Ok(stores);
    }
}