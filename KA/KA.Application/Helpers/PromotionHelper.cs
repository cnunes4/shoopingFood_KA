
using KA.Application.DTO;

namespace KA.Application.Helpers
{
    public static class  PromotionHelper
    {

        // Helper method to apply promotions to a single item
        public static decimal ApplyPromotions(List<PromotionDTO> promotions, ProductDTO item, List<ProductDTO> productsBasket)
        {
            decimal promotionDiscount = 0;
            int totalPromotionProduct = 0;
            var applicablePromotions = promotions.Where(p => p.ProductIdToApply == item.Id).ToList();

            foreach (var promotion in applicablePromotions)
            {
                totalPromotionProduct = productsBasket.FirstOrDefault(x => x.Id == promotion.ProductId).Quantity;
                int freeCount = totalPromotionProduct / promotion.QuantityProductId; 
                if (totalPromotionProduct >= promotion.QuantityProductId)
                {
                    promotionDiscount += CalculatePromotion(item.Price*item.Quantity, item.Price, promotion, freeCount);
                }
            }

            return promotionDiscount;
        } 

        private  static decimal CalculatePromotion(decimal priceTotal, decimal unitPrice, PromotionDTO appliedDiscount, int numToApply)
        {
            decimal discountAmount = 0;

            for (int i = 0; i< numToApply; i++)
            {
                discountAmount += unitPrice * appliedDiscount.Percentagem / 100;
                priceTotal -= discountAmount;
            }
           
            return discountAmount;
        }

    }
}
