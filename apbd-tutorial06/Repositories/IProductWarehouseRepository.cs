using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public interface IProductWarehouseRepository
{
    public Task<bool> IsOrderExist(int idOrder);
    public Task<int> InsertProduct(WarehouseDTO warehouseDto, double productPrice, int idOrder);
    public Task<int> InsertProductProcedure(WarehouseDTO warehouseDto);
}