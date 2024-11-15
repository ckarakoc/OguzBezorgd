using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Controllers;

public class StoresController(
    IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Store>>> GetStores()
    {
        var stores = await unitOfWork.StoreRepository.GetStoresAsync();
        return Ok(stores);
    }
    
    [HttpPost] 
    public async Task<ActionResult<IEnumerable<Store>>> CreateStore()
    {
        //todo
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<Store>>> GetStore(int id)
    {
        return Ok();
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<IEnumerable<Store>>> UpdateStore(int id)
    {
        return Ok();
    }
    
    [HttpGet("{id:int}/products")] 
    public async Task<ActionResult<IEnumerable<Store>>> GetStoreProducts(int id)
    {
        return Ok();
    }
    
    [HttpGet("{id:int}/products/{productId:int}")]
    public async Task<ActionResult<IEnumerable<Store>>> GetStoreProduct(int id, int productId)
    {
        return Ok();
    }
    
    [HttpPost("{id:int}/products/")]
    public async Task<ActionResult<IEnumerable<Store>>> AddStoreProduct(int id)
    {
        return Ok();
    }
}