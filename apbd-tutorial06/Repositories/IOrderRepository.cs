using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public interface IOrderRepository
{
    public Task<Order?> GetOrder(WarehouseDTO warehouseDto);
    public Task<bool> UpdateRecord(WarehouseDTO warehouseDto);
}