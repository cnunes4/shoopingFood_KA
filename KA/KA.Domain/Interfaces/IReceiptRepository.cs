using KA.Domain.Entities;

namespace KA.Domain.Interfaces
{
    public interface IReceiptRepository
    {
        Task<List<Receipt>?> GetAllReceiptsAsync();
        Task<int> AddReceiptAsync(Receipt receipt);
        Task<bool> AddAllProductsToReceiptAsync(List<Receiptsproduct> porducts, int receiptId);
        Task<List<Receipt>?> GetAllReceiptsByUserAsync(int idUser);
        Task<List<Receiptsproduct>?> GetAllDetailsByReceiptAsync(int idReceipt);
        Task<Receipt> GetReceiptAsync(int idReceipt);
    }
}
