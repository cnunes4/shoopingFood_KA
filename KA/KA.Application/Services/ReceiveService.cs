using AutoMapper;
using KA.Application.DTO;
using KA.Application.Helpers;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace KA.Application.Services
{
    public class ReceiveService : IReceiveService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReceiveService> _logger;
        private readonly IDiscountService _discountService;
        private readonly IPromotionService _promotionService;
        private readonly IProductService _productService;
        public ReceiveService(IReceiptRepository receiptRepository, IUserRepository userRepository, ILogger<ReceiveService> logger, 
            IDiscountService discountService, IPromotionService promotionService, IProductService productService)
        {
            _receiptRepository = receiptRepository;
            _userRepository = userRepository;
            _logger = logger;
            _discountService = discountService;
            _promotionService = promotionService;
            _productService = productService;
            // This configuration sets up a bidirectional mapping between the Receiptsproduct and ReceiptItemDTO classes.
            // and  Receipt and ReceiptDataDTO classes.
            // and  Receipt and ReceiptDTO classes.
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<ReceiptProduct, ReceiptItemDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDataDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDTO>().ReverseMap();
            }).CreateMapper();

        }

        /// <summary>
        /// Calculate the final price and the receipt details for the shop.
        /// </summary>
        /// <param name="basket">List of products in the basket.</param>
        /// <returns>ReceiptDataDTO with calculated totals.</returns>
        public async Task<ReceiptDataDTO?> CalculateReceiptAsync(BasketDTO basket, List<PromotionDTO> promotions, List<DiscountDTO> discounts)
        {
            var receipt = new ReceiptDataDTO();
            decimal totalBeforeDiscount = 0;
            decimal totalAfterDiscount = 0;


            // Apply discounts and promotions to each item and prepare the receipt
            foreach (var item in basket.Products)
            {
                var receiptItem = new ReceiptItemDTO
                {
                    ProductId = item.IdProduct,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Name = _productService.GetProductByIDAsync(item.IdProduct).Result.Name
                };

                decimal itemTotal = item.Price * item.Quantity;
                totalBeforeDiscount += itemTotal;

                // Apply discounts to the item
                decimal totalDiscount = (discounts != null ? DiscountHelper.ApplyDiscounts(item, discounts.Where(x=> x.ProductId== item.IdProduct).ToList()) : 0m);

                // Apply promotions to the item
                decimal totalPromotion = (promotions!= null ? PromotionHelper.ApplyPromotions(promotions, item, basket.Products) : 0m);

                // Set values in the receipt item
                receiptItem.PriceAfterDiscount = itemTotal - totalDiscount - totalPromotion;
                receiptItem.PriceBeforeDiscount = (item.Price* item.Quantity);
                receiptItem.TotalDiscount = totalDiscount+ totalPromotion;
                receipt.Items.Add(receiptItem);
            }

            // Calculate totals for the receipt
            totalAfterDiscount = receipt.Items.Sum(i => i.PriceAfterDiscount);
            receipt.TotalAfterDiscount = totalAfterDiscount;
            receipt.TotalBeforeDiscount = totalBeforeDiscount;
            receipt.TotalDiscount = totalBeforeDiscount - totalAfterDiscount;
            receipt.ReceiptDate = DateTime.Now;

            return receipt;
        }

        /// <summary>
        /// Generate a receipt with all information necessary to show to the client.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="basket">list of products in basket</param>
        /// <returns>ReceiptDataDTO with all calculated details or null in case of failure.</returns>
        public async Task<ReceiptDataDTO?> GenerateReceipt(string userName, BasketDTO basket, List<PromotionDTO> promotions, List<DiscountDTO> discounts)
        {
            try
            {
                // Calculate the receipt asynchronously
                var receipt = await CalculateReceiptAsync(basket, promotions, discounts);
                if (receipt == null)
                {
                    _logger.LogError("Error calculating receipt.");
                    return null;
                }

                // Get the user asynchronously
                var userIdResult = await _userRepository.GetUserAsync(userName);
                if (userIdResult == null)
                {
                    _logger.LogError("Username not found!");
                    return null;
                }

                // Create the receipt object to save
                var receiptToSave = new Receipt
                {
                    ReceiptDate = receipt.ReceiptDate,
                    TotalAfterDiscount = receipt.TotalAfterDiscount,
                    TotalBeforeDiscount = receipt.TotalBeforeDiscount,
                    UserId = userIdResult.UserId
                };

                // Add the receipt to the database
                var receiptIdSaved = await _receiptRepository.AddReceiptAsync(receiptToSave);
                if (receiptIdSaved <= 0)
                {
                    _logger.LogError("Failed to add receipt.");
                    return null;
                }

                // Add products to receipt
                var resultProducts = await _receiptRepository.AddAllProductsToReceiptAsync(
                    receipt.Items.Select(item => _mapper.Map<ReceiptProduct>(item)).ToList(),
                    receiptIdSaved);

                if (!resultProducts)
                {
                    _logger.LogError("Failed to add products to receipt.");
                    return null;
                }

                // Return the receipt data
                return receipt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating receipt.");
                return null;
            }
        }

        /// <summary>
        /// Get all receipts that one user has 
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>List with all receipts</returns>
        public async Task<List<ReceiptDataDTO>?> GetAllReceivesByUserId(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(userName);

                if (user!= null)
                {
                    var items = await _receiptRepository.GetAllReceiptsByUserAsync(user.UserId);
                    var listOfResult= new List<ReceiptDataDTO>();
                    foreach (var item in items)
                    {
                        var receipt = _mapper.Map<ReceiptDataDTO>(item);
                        receipt.TotalDiscount= receipt.TotalBeforeDiscount - receipt.TotalAfterDiscount;
                        listOfResult.Add(receipt);
                    }

                    return listOfResult;
                }
                _logger.LogError("user not found");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro generate get all receipt from user");
                return null;
            }
        }
        /// <summary>
        /// Get all products from one receipt 
        /// </summary>
        /// <param name="receiptId">receipt id in DB</param>
        /// <returns>List with all products</returns>
        public async Task<List<ReceiptItemDTO>> GetDetailsReceiptByReceiptId(int receiptId)
        {
            try
            {
                var result = await _receiptRepository.GetAllDetailsByReceiptAsync(receiptId);

                var listOfResult = new List<ReceiptItemDTO>();
                foreach (var item in result)
                {
                    var receipt = _mapper.Map<ReceiptItemDTO>(item);
                    receipt.Name = _productService.GetProductByIDAsync(item.ProductId).Result?.Name;
                    receipt.TotalDiscount = (item.Price* item.Quantity)-item.PriceAfterDiscount;
                    receipt.PriceBeforeDiscount = (item.Price * item.Quantity);
                    listOfResult.Add(receipt);
                }

                return listOfResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro get products by receipt id");
                return null;
            }
        }
        /// <summary>
        /// Get Details about one receipt
        /// </summary>
        /// <param name="receiptId">receipt id in DB</param>
        /// <returns>Details for receipt</returns>
        public async Task<ReceiptDTO> GetReceiptByReceiptId(int receiptId)
        {
            try
            {
                var result = await _receiptRepository.GetReceiptAsync(receiptId);
                if (result!= null)
                {
                    var item = _mapper.Map<ReceiptDTO>(result);
                    item.TotalDiscount= item.TotalBeforeDiscount-item.TotalAfterDiscount;
                    return item;
                }
                _logger.LogWarning($"no receipt for id {receiptId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro get details regarding a receipt id");
                return null;
            }
        }
    }
}
