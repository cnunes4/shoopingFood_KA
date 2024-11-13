using KA.Api1.Data;
using KA.Application.Interfaces;
using KAService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KA.Api1.Controllers
{
    [Route("api")]
    [ApiController]
    public class HistoryReceiveController : ControllerBase
    {
        private readonly ILogger<HistoryReceiveController> _logger;
        private readonly IReceiveService _receiveService;

        public HistoryReceiveController(ILogger<HistoryReceiveController> logger,IReceiveService receiveService)
        {
            _receiveService = receiveService;
            _logger = logger;
        }

        [HttpGet("GetAllReceiptsByUser")]
        [Authorize]
        public async Task<ActionResult<ListOfReceipts>> GetAllReceiptsByUser()
        {
            try
            {
                var result = await _receiveService.GetAllReceivesByUserId(User.Identity.Name);

                if (!result.Any())
                {
                    return NotFound("No Receipts");
                }

                return Ok(new ListOfReceipts()
                {
                    Receipts = result.OrderByDescending(x => x.ReceiptDate).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get all receipts.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpPost("GetDetailsReceiptById")]
        [Authorize]
        public async Task<ActionResult<DetailsOfReceipt>> GetDetailsReceiptById([FromBody] int receiptId)
        {
            try
            {
                var resultProducts = await _receiveService.GetDetailsReceiptByReceiptId(receiptId);

                if (!resultProducts.Any())
                {
                    return NotFound("No Details");
                }

                var resultReceipt = await _receiveService.GetReceiptByReceiptId(receiptId);

                if (resultReceipt == null)
                {
                    return NotFound("No Receipt");
                }
                return Ok( new DetailsOfReceipt(){
                   Products= resultProducts, 
                   Details = resultReceipt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get details about receipt.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
