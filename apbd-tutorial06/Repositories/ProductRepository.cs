using System.Data.Common;
using System.Data.SqlClient;
using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public class ProductRepository: IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Product?> GetProduct(int idProduct)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Product WHERE IdProduct = @IdProduct";
        command.Parameters.AddWithValue("@IdProduct", idProduct);
        
        await connection.OpenAsync();
        

        using (var dr = await command.ExecuteReaderAsync())
        {
            if (await dr.ReadAsync())
            {
                return new Product
                {
                    IdProduct = (int) dr["IdProduct"],
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Price = (double) dr["Price"]
                };
            }
        }

        return null;
    }
}