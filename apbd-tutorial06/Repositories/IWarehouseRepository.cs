using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetWarehouse(int idWarehouse);
}