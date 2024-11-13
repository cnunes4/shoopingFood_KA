using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoopingFood.Interfaces;
using ShoopingFood.Models;
using System.Text;

namespace ShoopingFood.Services
{
    public class ReceiptService : IReceiptService
    {

        private readonly ILogger<ReceiptService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceiptService(ILogger<ReceiptService> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("FoodApi");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ListOfReceipts?> GetAllReceiptsByUser()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/GetAllReceiptsByUser");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(jsonResponse);

               return jsonObject.ToObject<ListOfReceipts>();
            }

            return null;
        }


        public async Task<DetailsOfReceipt?> GetDetailsOfReceipt(int receiptId)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(receiptId), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/GetDetailsReceiptById", content);
            if (response.IsSuccessStatusCode)
            {

                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(jsonResponse);

                return jsonObject.ToObject<DetailsOfReceipt>();

            }

            return null;
        }
    }
}
