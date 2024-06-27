using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public interface IProductRepository
{
    Task<Product?> GetProduct(int idProduct);
}