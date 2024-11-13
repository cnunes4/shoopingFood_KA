using Newtonsoft.Json.Linq;
using ShoopingFood.Controllers;
using ShoopingFood.Interfaces;
using ShoopingFood.Models;
using System.Text;
using System.Text.Json;

namespace ShoopingFood.Services
{
    public class OrderService : IOrderService
    {

        private readonly ILogger<FoodController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(ILogger<FoodController> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("FoodApi");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Receipt?> ProcessOrder(ListOrderItem orderItems)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(orderItems), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/generate-receipt", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(jsonResponse);
                var receipt = jsonObject.ToObject<Receipt>();

                return receipt;
            }
            return null;
        }
    }
}
