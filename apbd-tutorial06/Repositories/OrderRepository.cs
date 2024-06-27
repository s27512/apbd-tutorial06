using System.Data.Common;
using System.Data.SqlClient;
using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Order?> GetOrder(WarehouseDTO warehouseDto)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT * FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt";
        command.Parameters.AddWithValue("@IdProduct", warehouseDto.IdProduct);
        command.Parameters.AddWithValue("@Amount", warehouseDto.Amount);
        command.Parameters.AddWithValue("@CreatedAt", warehouseDto.CreatedAt);

        await connection.OpenAsync();

        using (var dr = await command.ExecuteReaderAsync())
        {
            if (await dr.ReadAsync())
            {
                return new Order()
                {
                    IdOrder = (int)dr["IdOrder"],
                    IdProduct = (int)dr["IdProduct"],
                    Amount = (int)dr["Amount"],
                    CreatedAt = (DateTime)dr["CreatedAt"],
                    FulfilledAt = (DateTime)dr["FulfilledAt"]
                };
            }
        }

        return null;
    }

    public async Task<bool> UpdateRecord(WarehouseDTO warehouseDto)
    {
        var order = GetOrder(warehouseDto);
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var command = new SqlCommand();
        var time = DateTime.Now;

        command.Connection = connection;
        command.CommandText = "UPDATE [Order] SET FulfilledAt = @Time WHERE IdOrder = @IdOrder";
        command.Parameters.AddWithValue("@Time", time);
        command.Parameters.AddWithValue("@IdOrder", order.Id);
        await connection.OpenAsync();

        DbTransaction tran = await connection.BeginTransactionAsync();
        command.Transaction = (SqlTransaction)tran;
        try
        {
            await command.ExecuteNonQueryAsync();
            await tran.CommitAsync();
        }
        catch (SqlException exception)
        {
            await tran.RollbackAsync();
            return false;
        }
        catch (Exception exception)
        {
            await tran.RollbackAsync();
            return false;
        }

        return true;
    }
}