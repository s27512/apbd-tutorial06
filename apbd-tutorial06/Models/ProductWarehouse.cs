using System.ComponentModel.DataAnnotations;

namespace apbd_tutorial06.Models;

public class ProductWarehouse
{
    [Required]
    public int IdProductWarehouse { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdOrder { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}