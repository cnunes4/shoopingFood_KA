using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoopingFood.Interfaces;
using ShoopingFood.Models;
using System.Text;

namespace ShoopingFood.Services
{
    public class PromotionService : IPromotionService
    {

        private readonly ILogger<PromotionService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PromotionService(ILogger<PromotionService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("FoodApi");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ListOfPromotions?> GetAllPromotions()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/GetAllPromotions");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(jsonResponse);

               return jsonObject.ToObject<ListOfPromotions>();
            }

            return null;
        }
      
    }
}
