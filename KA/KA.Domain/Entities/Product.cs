using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public class Product
{
    [Required]
    public int IdProduct { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    public ICollection<DiscountProduct> DiscountProducts { get; set; }
}
