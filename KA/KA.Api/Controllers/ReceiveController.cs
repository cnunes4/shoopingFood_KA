using KA.Api1.Data;
using KA.Application.DTO;
using KA.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KA.Api1.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReceiveController : ControllerBase
    {
        private readonly ILogger<ReceiveController> _logger;
        private readonly IReceiveService _receiveService;
        private readonly IProductService _itemService;
        private readonly IDiscountService _discountService;
        private readonly IPromotionService _promotionService;
        public ReceiveController(ILogger<ReceiveController> logger,IReceiveService pricingService,
            IProductService itemService, IDiscountService discountService, IPromotionService promotionService)
        {
            _receiveService = pricingService;
            _itemService = itemService;
            _discountService = discountService;
            _promotionService = promotionService;
            _logger = logger;
        }

        [HttpPost("generate-receipt")]
        [Authorize]
        public async Task<IActionResult> GenerateReceipt([FromBody] ListOrderItem orderReceive)
        {
            try
            {
                var basket = new BasketDTO();
                var discounts = await _discountService.GetAllDiscountsAsync();
                var promotions = await _promotionService.GetAllPromotionsAsync();
                foreach (var order in orderReceive.Order)
                {
                    var item = await _itemService.GetItemByIDAsync(order.ProductID);

                    if (item != null && order.Quantity>0)
                    {
                        item.Quantity = order.Quantity;
                        basket.Products.Add(item);
                    }
                }

                // Generate the receipt
                var receipt = await _receiveService.GenerateReceipt(User.Identity.Name, basket, discounts, promotions);

                return Ok(receipt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the receipt.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
