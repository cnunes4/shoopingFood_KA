using KA.Application.DTO;

namespace KAService.Data
{
    public class DetailsOfReceipt
    {
        public ReceiptDTO Details { get; set; }
        public List<ReceiptItemDTO> Products { get; set; }
    }
}
