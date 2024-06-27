using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public class ProductWarehouseRepository: IProductWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public ProductWarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> IsOrderExist(int idOrder)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        command.Parameters.AddWithValue("@IdOrder", idOrder);

        await connection.OpenAsync();

        using (var dr = await command.ExecuteReaderAsync())
        {
            if (await dr.ReadAsync())
            {
                return true;
            }
        }
        return false;
    }

    public async Task<int> InsertProduct(WarehouseDTO warehouseDto, double productPrice, int idOrder)
    {
        var totalPrice = productPrice * warehouseDto.Amount; 
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var command = new SqlCommand();
        var time = DateTime.Now;

        command.Connection = connection;
        command.CommandText = "INSERT INTO Product_Warehouse (IdWarehouse,IdProduct,IdOrder,Amount,Price,CreatedAt) VALUES " +
                              "(@IdWarehouse,@IdProduct,@IdOrder,@Amount,@Price,@CreatedAt)";
        command.Parameters.AddWithValue("@IdWarehouse", warehouseDto.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", warehouseDto.IdProduct);
        command.Parameters.AddWithValue("@IdOrder", idOrder);
        command.Parameters.AddWithValue("@Amount", warehouseDto.Amount);
        command.Parameters.AddWithValue("@Price", totalPrice);
        command.Parameters.AddWithValue("@CreatedAt", time);
        await connection.OpenAsync();

        DbTransaction tran = await connection.BeginTransactionAsync();
        command.Transaction = (SqlTransaction)tran;
        try
        {
            int id = (int) await command.ExecuteScalarAsync();
            await tran.CommitAsync();
            return id;
        }
        catch (SqlException exception)
        {
            await tran.RollbackAsync();
            throw;
        }
        catch (Exception exception)
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task<int> InsertProductProcedure(WarehouseDTO warehouseDto)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                using (SqlCommand command = new SqlCommand("AddProductToWarehouse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@IdProduct", warehouseDto.IdProduct);
                    command.Parameters.AddWithValue("@IdWarehouse", warehouseDto.IdWarehouse);
                    command.Parameters.AddWithValue("@Amount", warehouseDto.Amount);
                    command.Parameters.AddWithValue("@CreatedAt", warehouseDto.CreatedAt);

                    int id = (int) await command.ExecuteScalarAsync();
                    return id;
                }
            }
        }
        catch (Exception e)
        {
            return -1;
        }
    }
}