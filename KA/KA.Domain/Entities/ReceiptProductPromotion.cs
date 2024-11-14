using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public class ReceiptProductPromotion
{
    [Required]
    public int ReceiptId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int PromotionId { get; set; }
}
