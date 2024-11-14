using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public class ReceiptProduct
{
    [Required]
    public int ReceiptId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal PriceAfterDiscount { get; set; }
}
