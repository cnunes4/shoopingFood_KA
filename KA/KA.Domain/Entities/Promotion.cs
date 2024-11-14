using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public  class Promotion
{
    [Required]
    public int IdPromotion { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 1)]
    public string Description { get; set; }

    [Required]
    public int ProductIdToApply { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public int Percentage { get; set; }

    [Required]
    public DateTime DateStart { get; set; }

    [Required]
    public DateTime DateEnd { get; set; }

    [Required]
    public bool IsEnabled { get; set; }
}
