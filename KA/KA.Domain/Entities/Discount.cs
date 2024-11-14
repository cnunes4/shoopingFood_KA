using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public  class Discount
{
    [Required]
    public int DiscountId { get; set; }

    [Required]
    [StringLength(100)]
    public string Description { get; set; }

    [Required]
    public int Percentage { get; set; }

    [Required]
    public bool IsEnabled { get; set; }

    public ICollection<DiscountProduct> DiscountProducts { get; set; }
}
