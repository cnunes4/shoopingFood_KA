using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public class ReceiptProductDiscount
{
    [Required]
    public int ReceiptId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int DiscountId { get; set; }

    public Discount Discount { get; set; } // Navigation property
    public Product Product { get; set; } // Navigation property
}