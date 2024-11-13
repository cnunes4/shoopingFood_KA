
using KA.Application.DTO;

namespace KA.Application.Helpers
{
    public static class DiscountHelper
    {
        // Helper method to apply discounts to a single item
        public static decimal ApplyDiscounts(ProductDTO item, List<DiscountDTO> discounts)
        {
            decimal totalDiscount = 0;
            foreach (var discount in discounts.Where(x => x.ItemToApply == item.Id))
            {
                totalDiscount += CalculateDiscount(item.Price * item.Quantity, discount);
            }
            return totalDiscount;
        }
 
        // Apply the discount to the total price
        private static decimal CalculateDiscount(decimal priceTotal, DiscountDTO appliedDiscount)
        {
            decimal discountAmount = 0;

            discountAmount = priceTotal * appliedDiscount.Percentage / 100;
            priceTotal -= discountAmount;
            return discountAmount;
        }
    }
}
