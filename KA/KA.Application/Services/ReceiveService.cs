using AutoMapper;
using KA.Application.DTO;
using KA.Application.Helpers;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace KA.Application.Services
{
    public class ReceiveService : IReceiveService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReceiveService> _logger;
        public ReceiveService(IReceiptRepository receiptRepository, IUserRepository userRepository, ILogger<ReceiveService> logger)
        {
            _receiptRepository = receiptRepository;
            _userRepository = userRepository;
            _logger = logger;
            // This configuration sets up a bidirectional mapping between the Receiptsproduct and ReceiptItemDTO classes.
            // and  Receipt and ReceiptDataDTO classes.
            // and  Receipt and ReceiptDTO classes.
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Receiptsproduct, ReceiptItemDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDataDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDTO>().ReverseMap();
            }).CreateMapper();

        }

        /// <summary>
        /// Calculate the final price and the receipt details for shop
        /// </summary>
        /// <param name="basket">List of products in the basket</param>
        /// <param name="discounts">All discounts in DB</param>
        /// <param name="promotions">All promotion in BD</param>
        /// <returns></returns>
        public async Task<ReceiptDataDTO?> CalculateReceipt(BasketDTO basket, List<DiscountDTO> discounts, List<PromotionDTO> promotions)
        {
            var receipt = new ReceiptDataDTO();
            decimal totalBeforeDiscount = 0;
            decimal totalAfterDiscount = 0;

            // Apply discounts and promotions to each item and prepare the receipt
            foreach (var item in basket.Products)
            {
                var receiptItem = new ReceiptItemDTO
                {
                    ProductId = item.Id,
                    Name = item.Name,
                    Price = item.Price ,
                    Quantity = item.Quantity
                };

                decimal itemTotal = item.Price * item.Quantity;
                totalBeforeDiscount += itemTotal;

                // Apply discounts to the item
                decimal totalDiscount = DiscountHelper.ApplyDiscounts(item, discounts);

                // Apply promotions to the item
                decimal totalPromotion = PromotionHelper.ApplyPromotions(promotions, item, basket.Products);

                decimal totalAfterItemDiscount = itemTotal - totalDiscount - totalPromotion;

                // Set values in the receipt item
                receiptItem.TotalDiscount = totalDiscount + totalPromotion;
                receiptItem.PriceAfterDiscount = totalAfterItemDiscount;
                receiptItem.PriceBeforeDiscount = itemTotal;
                // Add the item to the receipt
                receipt.Items.Add(receiptItem);
            }

            // Calculate totals for the receipt
            totalAfterDiscount = receipt.Items.Sum(i => i.PriceAfterDiscount);
            receipt.TotalAfterDiscount = totalAfterDiscount;
            receipt.TotalDiscount = receipt.Items.Sum(i => i.TotalDiscount);
            receipt.TotalBeforeDiscount = totalBeforeDiscount;
            receipt.ReceiptDate = DateTime.Now;

            return receipt;
                 
        }

        /// <summary>
        /// Generate a receipt with all information necessary to show to the client
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="basket">list of products in basket </param>
        /// <param name="discounts">all discount in DB</param>
        /// <param name="promotions">all promotion in BD</param>
        /// <returns></returns>
        public async Task<ReceiptDataDTO?> GenerateReceipt(string username, BasketDTO basket, List<DiscountDTO> discounts, List<PromotionDTO> promotions)
        {
            try
            {
                var receipt= this.CalculateReceipt(basket,discounts,promotions).Result;
                if (receipt!= null)
                {
                    var userIdresult = await _userRepository.GetUserAsync(username);

                    if (userIdresult != null)
                    {
                        var receiptToSvae = new Receipt()
                        {
                            ReceiptDate = receipt.ReceiptDate,
                            TotalAfterDiscount = receipt.TotalAfterDiscount,
                            TotalBeforeDiscount = receipt.TotalBeforeDiscount,
                            TotalDiscount = receipt.TotalDiscount,
                            UserId = userIdresult.UserId
                        };

                        // Call the service to add a new receipt
                        var result = await _receiptRepository.AddReceiptAsync(receiptToSvae);
                        if (result > 0)
                        {
                            var listToAdd = new List<Receiptsproduct>();
                            foreach (var item in receipt.Items)
                            {
                                listToAdd.Add(_mapper.Map<Receiptsproduct>(item));
                            }
                            var resultProducts = await _receiptRepository.AddAllProductsToReceiptAsync(listToAdd, result);

                            if (resultProducts)
                            {
                                return receipt;
                            }
                            _logger.LogError($"Somethigs happen with add produts to receipt");
                            return null;
                        }

                    }
                    _logger.LogError($"Username not found!");
                    return null;
                }
                _logger.LogError($"Error calculate receipt!");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro generate receipt");
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

                return user != null
                    ? (await _receiptRepository.GetAllReceiptsByUserAsync(user.UserId))
                        .Select(item => _mapper.Map<ReceiptDataDTO>(item))
                        .ToList()
                    : null;
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

                return result?.Select(item => _mapper.Map<ReceiptItemDTO>(item)).ToList();
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
                return result != null ? _mapper.Map<ReceiptDTO>(result) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro get details regarding a receipt id");
                return null;
            }
        }
    }
}
