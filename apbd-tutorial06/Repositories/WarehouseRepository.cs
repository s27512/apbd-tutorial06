using System.Data.Common;
using System.Data.SqlClient;
using apbd_tutorial06.Models;

namespace apbd_tutorial06.Repositories;

public class WarehouseRepository: IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Warehouse?> GetWarehouse(int idWarehouse)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
        
        await connection.OpenAsync();

        using (var dr = await command.ExecuteReaderAsync())
        {
            if (await dr.ReadAsync())
            {
                return new Warehouse()
                {
                    IdWarehouse = (int) dr["IdWarehouse"],
                    Name = dr["Name"].ToString(),
                    Address = dr["Address"].ToString()
                };
            }
        }
        return null;
    }
}