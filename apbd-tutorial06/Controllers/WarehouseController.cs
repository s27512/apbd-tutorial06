using apbd_tutorial06.Models;
using apbd_tutorial06.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tutorial06.controller;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController: ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> addRecordProductWarehouse(WarehouseDTO warehouseDto)
    {
        var isProductExists = await _warehouseService.IsProductExists(warehouseDto.IdProduct);
        
        if (!isProductExists)
        {
            return BadRequest("Product with given ID does not exist.");
        }
        
        var isWarehouseExists = await _warehouseService.IsWarehouseExists(warehouseDto.IdWarehouse);

        if (!isWarehouseExists)
        {
            return BadRequest("Warehouse with given ID does not exists.");
        }

        if (warehouseDto.Amount <= 0)
        {
            return BadRequest("Amount must be bigger than 0");
        }

        var isOrderExists = await _warehouseService.IsOrderExists(warehouseDto);
        if (!isOrderExists)
        {
            return BadRequest("The order does not exist.");
        }

        var isOrderCompleted = await _warehouseService.IsOrderCompleted(warehouseDto);
        if (isOrderCompleted)
        {
            return BadRequest("The order is already completed.");
        }

        var idRecord = await _warehouseService.UpdateAndInsert(warehouseDto);

        if (idRecord != -1)
        {
            return Ok(new {IdProductWarehouse = idRecord});
        }

        return BadRequest("An error occured during insertion.");

    }

    [HttpPost]
    public async Task<IActionResult> addProductWarehouseProcedure(WarehouseDTO warehouseDto)
    {
        int idRecord = await _warehouseService.UpdateAndInsert(warehouseDto);
        if (idRecord != -1)
        {
            return Ok(new { IdProductWarehouse = idRecord });
        }

        return BadRequest("An error occured during insertion.");
    }
}