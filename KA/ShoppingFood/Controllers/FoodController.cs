using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ShoopingFood.Interfaces;
using ShoopingFood.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;

namespace ShoopingFood.Controllers
{
    [Authorize]
    public class FoodController : Controller
    {
        private readonly ILogger<FoodController> _logger;
        private readonly IProductsService _productsService;
        private readonly IPromotionService _promotionService;
        private readonly IOrderService _orderService;
        

        public FoodController(ILogger<FoodController> logger, IOrderService orderService, IProductsService productsService, IPromotionService promotionService)
        {
            _logger = logger;
            _productsService = productsService;
            _orderService = orderService;
            _promotionService = promotionService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var items = await _productsService.GetListOfItens();
            return View(items);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitOrder(Dictionary<int, int> Quantities)
        {
            var orderItems = new List<OrderItem>();

            foreach (var quantity in Quantities)
            {
                orderItems.Add(new OrderItem
                {
                    ProductID = quantity.Key,
                    Quantity = quantity.Value
                });
            }

            var list = new ListOrderItem()
            {
                Order = orderItems
            };
            var receipt= await _orderService.ProcessOrder(list);

            return View("Receipt", receipt);
        }
    }
}
