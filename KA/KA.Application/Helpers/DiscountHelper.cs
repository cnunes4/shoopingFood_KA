
using KA.Application.DTO;

namespace KA.Application.Helpers
{
    public static class DiscountHelper
    {
        /// <summary>
        /// Helper method to apply discounts to a single item
        /// </summary>
        /// <param name="item">Item in the basket</param>
        /// <param name="discounts">List of discounts to apply</param>
        /// <returns>Total price of discounts to apply</returns>
        public static decimal ApplyDiscounts(ProductDTO item, List<DiscountDTO> discounts)
        {
            return discounts
                .Where(discount => discount.ItemToApply == item.Id)
                .Sum(discount => CalculateDiscount(item.Price * item.Quantity, discount));
        }

        /// <summary>
        /// Calculate and return the discount amount
        /// </summary>
        /// <param name="totalPrice">Total price (price of product * quantity)</param>
        /// <param name="discount">Discount to apply</param>
        /// <returns>Total discounts to apply</returns>
        private static decimal CalculateDiscount(decimal totalPrice, DiscountDTO discount)
        {
            return totalPrice * discount.Percentage / 100;
        }
    }
}
