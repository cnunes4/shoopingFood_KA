using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public  class Receipt
{
    public int IdReceipt { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public decimal TotalBeforeDiscount { get; set; }

    [Required]
    public decimal TotalAfterDiscount { get; set; }

    [Required]
    public DateTime ReceiptDate { get; set; }
}
