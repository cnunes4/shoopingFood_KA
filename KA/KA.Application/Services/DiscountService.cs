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

            if (items == null || !items.Any())
            {
                _logger.LogError($"No Discounts found");
                return null;
            }


            return items.Select(item => _mapper.Map<DiscountDTO>(item)).ToList();
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
                _logger.LogError($"No Discounts found o this product {id}");
                return null;
            }
            return items.Select(item => _mapper.Map<DiscountDTO>(item)).ToList(); ;
        }
    }
}
