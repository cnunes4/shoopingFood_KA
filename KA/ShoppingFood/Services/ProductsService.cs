using Newtonsoft.Json.Linq;
using ShoopingFood.Interfaces;
using ShoopingFood.Models;

namespace ShoopingFood.Services
{
    public class ProductsService : IProductsService
    {

        private readonly ILogger<ProductsService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsService(ILogger<ProductsService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("FoodApi");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ListOfFood?> GetListOfItens()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/GetAllProducts");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(jsonResponse);

               return jsonObject.ToObject<ListOfFood>();
            }

            return null;
        }
    }
}
