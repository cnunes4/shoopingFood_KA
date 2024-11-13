
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace KA.Infra.Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly KADbContext _context;

        public ReceiptRepository(KADbContext context)
        {
            _context = context;
        }

        public async Task<List<Receipt>?> GetAllReceiptsAsync()
        {
            var items = await _context.Receipts.ToListAsync();

            if (!items.Any())
            {
                return null;
            }

            return items;
        }


        public async Task<int> AddReceiptAsync(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            //Get NEW ReceiptId
            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return receipt.Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> AddAllProductsToReceiptAsync(List<Receiptsproduct> porducts, int receiptId)
        {
            try
            {
                //Add Products to Receipt
                foreach (var item in porducts)
                {
                    var product = new Receiptsproduct()
                    {
                        Name = item.Name,
                        Price = item.Price,
                        PriceAfterDiscount = item.PriceAfterDiscount,
                        TotalDiscount = item.TotalDiscount,
                        Quantity = item.Quantity,
                        ReceiptId = receiptId,
                        ProductId = item.ProductId,
                    };

                    _context.Receiptsproducts.AddRange(product);
                }
                int rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Get all Receipts using a id user 
        /// </summary>
        /// <param name="date">The date the receipt was created.</param>
        /// <param name="userId">The ID of the user associated with the receipt.</param>
        /// <param name="receiptProducts">A list of products included in the receipt.</param>
        /// <returns>A redirect to the receipt details page for the newly created receipt.</returns>
        public async Task<List<Receipt>?> GetAllReceiptsByUserAsync(int idUser)
        {
            var items = await _context.Receipts.Where(x => x.UserId == idUser).ToListAsync();

            if (!items.Any())
            {
                return null;
            }
            return items;
        }


        public async Task<List<Receiptsproduct>?> GetAllDetailsByReceiptAsync(int idReceipt)
        {
            var items = await _context.Receiptsproducts.Where(x => x.ReceiptId == idReceipt).ToListAsync();

            if (!items.Any())
            {
                return null;
            }
            return items;
        }


        public async Task<Receipt> GetReceiptAsync(int idReceipt)
        {
            var item = await _context.Receipts.Where(x => x.Id == idReceipt).FirstOrDefaultAsync();

            if (item== null)
            {
                return null;
            }
            return item;
        }

    }
}
