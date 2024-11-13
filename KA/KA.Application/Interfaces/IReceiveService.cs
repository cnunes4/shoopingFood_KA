
using KA.Application.DTO;

namespace KA.Application.Interfaces
{
    public interface IReceiveService
    {
        Task<ReceiptDataDTO?> GenerateReceipt(string userName, BasketDTO basket, List<DiscountDTO> discounts, List<PromotionDTO> promotions);
        Task<List<ReceiptDataDTO>?> GetAllReceivesByUserId(string userName);
        Task<List<ReceiptItemDTO>> GetDetailsReceiptByReceiptId(int receiptId);
        Task<ReceiptDTO> GetReceiptByReceiptId(int receiptId);
    }
}
