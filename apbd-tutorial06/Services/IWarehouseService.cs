using apbd_tutorial06.Models;

namespace apbd_tutorial06.Services;

public interface IWarehouseService
{
    public Task<bool> IsProductExists(int idProduct);
    public Task<bool> IsWarehouseExists(int idWarehouse);
    public Task<bool> IsOrderExists(WarehouseDTO warehouseDto);
    public Task<bool> IsOrderCompleted(WarehouseDTO warehouseDto);
    public Task<int> UpdateAndInsert(WarehouseDTO warehouseDtO);
    public Task<int> InsertProductProcedure(WarehouseDTO warehouseDto);

}