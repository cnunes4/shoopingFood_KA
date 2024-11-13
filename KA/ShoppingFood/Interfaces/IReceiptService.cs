using ShoopingFood.Models;

namespace ShoopingFood.Interfaces
{
    public interface IReceiptService
    {
        Task<ListOfReceipts?> GetAllReceiptsByUser();
        Task<DetailsOfReceipt?> GetDetailsOfReceipt(int receiptId);
    }

}
