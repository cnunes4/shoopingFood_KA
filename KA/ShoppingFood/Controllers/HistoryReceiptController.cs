using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoopingFood.Interfaces;

namespace ShoopingFood.Controllers
{
    [Authorize]
    public class HistoryReceiptController : Controller
    {
        private readonly ILogger<HistoryReceiptController> _logger;
        private readonly IReceiptService _receiptService;
        

        public HistoryReceiptController(ILogger<HistoryReceiptController> logger, IReceiptService receiptService)
        {
            _logger = logger;
            _receiptService = receiptService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var items = await _receiptService.GetAllReceiptsByUser();
            return View(items);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDetails(int receiptId)
        {
            var details= await _receiptService.GetDetailsOfReceipt(receiptId);

            return View("Details", details);
        }
    }
}
