using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KA.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            // This configuration sets up a bidirectional mapping between the Discount and DiscountDTO classes.
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Discount, DiscountDTO>().ReverseMap();
            }).CreateMapper();
        }

        /// <summary>
        /// Get All Discounts in DB
        /// </summary>
        /// <returns>List of discounts</returns>
        public async Task<List<DiscountDTO>?> GetAllDiscountsAsync()
        {
            var items = await _discountRepository.GetAllDiscountsAsync();
            var discountProducts = await _discountRepository.GetDiscountsForEachProductAsync();

            var listToReturn = items
                .SelectMany(item => discountProducts
                    .Where(product => product.DiscountId == item.DiscountId)
                    .Select(product =>
                    {
                        var discount = _mapper.Map<DiscountDTO>(item);
                        discount.ProductId = product.ProductId;
                        return discount;
                    }))
                .ToList();


            if (items == null || !items.Any())
            {
                _logger.LogWarning($"No Discounts found");
                return null;
            }


            return listToReturn;
        }

        /// <summary>
        /// Get all discounts for one product 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of discounts</returns>
        public async Task<List<DiscountDTO>?> GetDiscountsByProductIdAsync(int id)
        {
            var items = await _discountRepository.GetDiscountsByProductIdAsync(id);

            if (items == null || !items.Any())
            {
                _logger.LogWarning($"No Discounts found o this product {id}");
                return null;
            }
            var itensToReturn= new List<DiscountDTO>();
            foreach (var item in items)
            {
                var discount = _mapper.Map<DiscountDTO>(item);
                discount.ProductId = id;
                itensToReturn.Add(discount);
            }
            return itensToReturn;
        }
    }
}
