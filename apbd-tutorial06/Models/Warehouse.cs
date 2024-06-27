using System.ComponentModel.DataAnnotations;

namespace apbd_tutorial06.Models;

public class Warehouse
{
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
}