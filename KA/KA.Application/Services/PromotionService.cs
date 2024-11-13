using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;

namespace KA.Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IMapper _mapper;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Promotion, PromotionDTO>().ReverseMap();
            }).CreateMapper();
        }


        public async Task<List<PromotionDTO>?> GetAllPromotionsAsync()
        {
             var items = await _promotionRepository.GetAllPromotionsAsync();

            if ( !items.Any())
            {
                return null;
            }

            var itemsDTO = new List<PromotionDTO>();
            foreach (var item in items)
            {
                itemsDTO.Add(_mapper.Map<PromotionDTO>(item));
            }

            return itemsDTO;
        }



     
    }
}
