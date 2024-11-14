using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KA.Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PromotionService> _logger;

        public PromotionService(IPromotionRepository promotionRepository, ILogger<PromotionService> logger)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;

            // This configuration sets up a bidirectional mapping between the Promotion and PromotionDTO classes.
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Promotion, PromotionDTO>().ReverseMap();
            }).CreateMapper();
        }

        /// <summary>
        /// Get all promotions in DB
        /// </summary>
        /// <returns>List of promotions</returns>
        public async Task<List<PromotionDTO>?> GetAllPromotionsAsync()
        {
             var items = await _promotionRepository.GetAllPromotionsAsync();

            if (items == null || !items.Any())
            {
                _logger.LogWarning($"No Promotions found");
                return null;
            }

            return items.Select(item => _mapper.Map<PromotionDTO>(item)).ToList();
        }
    }
}
