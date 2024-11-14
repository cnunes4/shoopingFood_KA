using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public class DiscountProduct
{
    [Required]
    public int DiscountId { get; set; }

    public Discount Discount { get; set; }

    [Required]
    public int ProductId { get; set; }

    public Product Product { get; set; }
}
