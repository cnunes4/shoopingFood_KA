using KA.Application.Interfaces;
using KAService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KA.Api1.Controllers
{
    [Route("api")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly ILogger<PromotionsController> _logger;

        public PromotionsController(ILogger<PromotionsController> logger, IPromotionService promotionService)
        {
            _promotionService = promotionService;
            _logger = logger;
        }

        /// <summary>
        /// Get all promotions 
        /// </summary>
        /// <returns>List of promotions</returns>
        [HttpGet("GetAllPromotions")]
        [Authorize]
        public async Task<ActionResult<ListOfPromotions>> GetAllPromotions()
        {
            try
            {
                var result = await _promotionService.GetAllPromotionsAsync();

                if (!result.Any())
                {
                     return NotFound("No Promotions");
                }

                return Ok(new ListOfPromotions(){
                   PromotionsAvailable= result
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
