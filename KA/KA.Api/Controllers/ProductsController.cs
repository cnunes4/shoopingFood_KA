using KA.Application.DTO;
using KA.Application.Interfaces;
using KAService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KA.Api1.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;
        private readonly IPromotionService _promotionService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger, IProductService productsService, IPromotionService promotionService)
        {
            _productsService = productsService;
            _logger = logger;
            _promotionService = promotionService;
        }

        [HttpGet("GetAllProducts")]
        [Authorize]
        public async Task<ActionResult<ListOfFood>> GetAllProducts()
        {
            try
            {
                var resultProducts = await _productsService.GetAllProductsAsync();

                if (!resultProducts.Any())
                {
                    return NotFound("No Products");
                }

                var resultPromotions = await _promotionService.GetAllPromotionsAsync();

                if (!resultPromotions.Any())
                {
                    return NotFound("No Promotions");
                }

                return Ok(new ListOfFood(){
                   FoodAvailable= resultProducts, 
                   Promotions = resultPromotions
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get all products.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
