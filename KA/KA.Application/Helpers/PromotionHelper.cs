
using KA.Application.DTO;

namespace KA.Application.Helpers
{
    public static class  PromotionHelper
    {
        /// <summary>
        /// Helper method to apply promotions to a single item
        /// </summary>
        /// <param name="promotions">All promotions</param>
        /// <param name="item">Item in the bascket</param>
        /// <param name="productsBasket">Products list in the bascket</param>
        /// <returns>Total promotions discount to aaply</returns>
        public static decimal ApplyPromotions(List<PromotionDTO> promotions, ProductDTO item, List<ProductDTO> productsBasket)
        {
            decimal totalPromotionDiscount = 0;
            // Filters applicable promotions for the current item
            var applicablePromotions = promotions.Where(p => p.ProductIdToApply == item.IdProduct).ToList();

            foreach (var promotion in applicablePromotions)
            {
                // Checks the quantity of the product in the basket
                int totalPromotionProduct = productsBasket.FirstOrDefault(x => x.IdProduct == promotion.ProductId)?.Quantity ?? 0;

                // Calculates how many times the promotion can be applied
                int applicableCount = totalPromotionProduct / promotion.Quantity;

                // Applies the discount and accumulates the total promotion discount
                if (applicableCount > 0)
                {
                    // Aplica o desconto e acumula o valor total da promoção
                    totalPromotionDiscount += CalculatePromotion(item.Price * item.Quantity, item.Price, promotion, applicableCount);
                }
            }

            return totalPromotionDiscount;
        }

        /// <summary>
        /// Method to calculate the discount amount for the given promotion
        /// </summary>
        /// <param name="totalPrice">Total price (price of product * quantity)</param>
        /// <param name="unitPrice">unique price for product</param>
        /// <param name="promotion">Promotion to apply</param>
        /// <param name="numApplications">Number of times we need to apply one promotion</param>
        /// <returns>total discount to aaply/returns>
        private static decimal CalculatePromotion(decimal totalPrice, decimal unitPrice, PromotionDTO promotion, int numApplications)
        {
            decimal totalDiscountAmount = 0;
            // Applies the discount based on the number of times the promotion is applied.
            for (int i = 0; i < numApplications; i++)
            {
                totalDiscountAmount += unitPrice * promotion.Percentage / 100;
            }

            return totalDiscountAmount;
        }

    }
}
