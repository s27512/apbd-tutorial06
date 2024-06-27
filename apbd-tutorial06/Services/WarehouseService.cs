using apbd_tutorial06.Models;
using apbd_tutorial06.Repositories;

namespace apbd_tutorial06.Services;

public class WarehouseService: IWarehouseService
{
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;
    private IWarehouseRepository _warehouseRepository;
    private IProductWarehouseRepository _productWarehouseRepository;

    public WarehouseService(IProductRepository productRepository, IOrderRepository orderRepository, IWarehouseRepository warehouseRepository, IProductWarehouseRepository productWarehouseRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _warehouseRepository = warehouseRepository;
        _productWarehouseRepository = productWarehouseRepository;
    }

    public async Task<bool> IsProductExists(int idProduct)
    {
        var product = await _productRepository.GetProduct(idProduct);
        return product != null;
        
    }

    public async Task<bool> IsWarehouseExists(int idWarehouse)
    {
        var warehouse = await _warehouseRepository.GetWarehouse(idWarehouse);
        return warehouse != null;
    }

    public async Task<bool> IsOrderExists(WarehouseDTO warehouseDto)
    {
        var order = await _orderRepository.GetOrder(warehouseDto);
        return order != null;
    }


    public async Task<bool> IsOrderCompleted(WarehouseDTO warehouseDto)
    {
        var order = await _orderRepository.GetOrder(warehouseDto);
        var idOrder = order.IdOrder;
        return await _productWarehouseRepository.IsOrderExist(idOrder);

    }

    public async Task<int> UpdateAndInsert(WarehouseDTO warehouseDto)
    {
        var order = await _orderRepository.GetOrder(warehouseDto);
        var isUpdated = await _orderRepository.UpdateRecord(warehouseDto);
        var product = await _productRepository.GetProduct(warehouseDto.IdProduct);
        
        if (isUpdated)
        {
            return await _productWarehouseRepository.InsertProduct(warehouseDto,product.Price,order.IdOrder);
        }

        return -1;
    }

    public async Task<int> InsertProductProcedure(WarehouseDTO warehouseDto)
    {
        int id = await _productWarehouseRepository.InsertProductProcedure(warehouseDto);
        return id;
    }
}