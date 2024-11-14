﻿
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace KA.Infra.Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly KADbContext _context;
        private readonly ILogger<ReceiptRepository> _logger;
        public ReceiptRepository(KADbContext context, ILogger<ReceiptRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        /// <summary>
        /// Get all receipts in BD
        /// </summary>
        /// <returns>List fo receipts
        public async Task<List<Receipt>?> GetAllReceiptsAsync()
        {
            var items = await _context.Receipts.ToListAsync();

            if (items == null || !items.Any())
            {
                _logger.LogError($"No Receipts found");
                return null;
            }

            return items;
        }

        /// <summary>
        /// Add receipt in DB
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns>id of new receipt</returns>
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
                _logger.LogError($"Something happened with insert receipt in DB");
                return 0;
            }
        }

        /// <summary>
        /// Add product for one receipt
        /// </summary>
        /// <param name="porducts"></param>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public async Task<bool> AddAllProductsToReceiptAsync(List<Receiptsproduct> products, int receiptId)
        {
            try
            {
                var receiptProducts = products.Select(item => new Receiptsproduct
                {
                    Name = item.Name,
                    Price = item.Price,
                    PriceAfterDiscount = item.PriceAfterDiscount,
                    TotalDiscount = item.TotalDiscount,
                    Quantity = item.Quantity,
                    ReceiptId = receiptId,
                    ProductId = item.ProductId,
                }).ToList();

                _context.Receiptsproducts.AddRange(receiptProducts);
                int rowsAffected = await _context.SaveChangesAsync();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro insert all products in DB for one receipt");
                return false;
            }
        }


        /// <summary>
        /// Get all Receipts using a id user 
        /// </summary>
        /// <param name="userId">The ID of the user associated with the receipt.</param>
        /// <returns>A redirect to the receipt details page for the newly created receipt.</returns>
        public async Task<List<Receipt>?> GetAllReceiptsByUserAsync(int idUser)
        {
            var items = await _context.Receipts.Where(x => x.UserId == idUser).ToListAsync();

            if (items == null || !items.Any())
            {
                _logger.LogError($"No Receipts found for user {idUser}");
                return null;
            }
            return items;
        }

        /// <summary>
        /// Get all product for one receipt
        /// </summary>
        /// <param name="idReceipt">receipt id </param>
        /// <returns>List of all product for one receipt</returns>
        public async Task<List<Receiptsproduct>?> GetAllDetailsByReceiptAsync(int idReceipt)
        {
            var items = await _context.Receiptsproducts.Where(x => x.ReceiptId == idReceipt).ToListAsync();

            if (items == null || !items.Any())
            {
                _logger.LogError($"No products found for receipt {idReceipt}");
                return null;
            }
            return items;
        }

        /// <summary>
        /// Get Details of one receipt
        /// </summary>
        /// <param name="idReceipt">Receipt id</param>
        /// <returns>All details about receipt</returns>
        public async Task<Receipt> GetReceiptAsync(int idReceipt)
        {
            var item = await _context.Receipts.Where(x => x.Id == idReceipt).FirstOrDefaultAsync();

            if (item == null)
            {
                _logger.LogError($"No Receipt found for receipt {idReceipt}");
                return null;
            }
            return item;
        }

    }
}
