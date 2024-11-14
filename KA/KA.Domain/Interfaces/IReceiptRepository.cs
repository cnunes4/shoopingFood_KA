using KA.Domain.Entities;

namespace KA.Domain.Interfaces
{
    public interface IReceiptRepository
    {
        Task<List<Receipt>?> GetAllReceiptsAsync();
        Task<int> AddReceiptAsync(Receipt receipt);
        Task<bool> AddAllProductsToReceiptAsync(List<ReceiptProduct> porducts, int receiptId);
        Task<List<Receipt>?> GetAllReceiptsByUserAsync(int idUser);
        Task<List<ReceiptProduct>?> GetAllDetailsByReceiptAsync(int idReceipt);
        Task<Receipt> GetReceiptAsync(int idReceipt);
        Task<bool> AddAllPromotionsToReceiptAsync(List<Promotion> promotions, int receiptId, int productId);
        Task<bool> AddAllDiscountsToReceiptAsync(List<Discount> discounts, int receiptId, int productId);
    }
}
