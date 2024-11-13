using AutoMapper;
using KA.Application.DTO;
using KA.Application.Helpers;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace KA.Application.Services
{
    public class ReceiveService : IReceiveService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ReceiveService(IReceiptRepository receiptRepository, IUserRepository userRepository)
        {
            _receiptRepository = receiptRepository;
            _userRepository = userRepository;
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Receiptsproduct, ReceiptItemDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDataDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDTO>().ReverseMap();
            }).CreateMapper();

        }


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
                            return null;
                        }

                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<List<ReceiptDataDTO>?> GetAllReceivesByUserId(string userName)
        {
            try
            {
                var userIdresult = await _userRepository.GetUserAsync(userName);

                if (userIdresult != null)
                {
                    var result = await _receiptRepository.GetAllReceiptsByUserAsync(userIdresult.UserId);
                    var listOfResult = new List<ReceiptDataDTO>();
                    foreach (var item in result)
                    {

                        var receipt = _mapper.Map<ReceiptDataDTO>(item);
                        listOfResult.Add(receipt);
                    }

                    return listOfResult;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ReceiptItemDTO>> GetDetailsReceiptByReceiptId(int receiptId)
        {
            try
            {
                var result = await _receiptRepository.GetAllDetailsByReceiptAsync(receiptId);

                if (result != null)
                {
                    var listOfResult = new List<ReceiptItemDTO>();
                    foreach (var item in result)
                    {
                        var receipt = _mapper.Map<ReceiptItemDTO>(item);
                        listOfResult.Add(receipt);
                    }

                    return listOfResult;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ReceiptDTO> GetReceiptByReceiptId(int receiptId)
        {
            try
            {
                var result = await _receiptRepository.GetReceiptAsync(receiptId);

                if (result != null)
                {
                    return _mapper.Map<ReceiptDTO>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
