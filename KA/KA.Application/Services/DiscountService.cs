using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using System.Collections.Generic;

namespace KA.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;


        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Discount, DiscountDTO>().ReverseMap();
            }).CreateMapper();
        }

        public async Task<List<DiscountDTO>?> GetAllDiscountsAsync()
        {
            var items = await _discountRepository.GetAllDiscountsAsync();

            if (!items.Any())
            {
                return null;
            }

            var itemsDTO = new List<DiscountDTO>();
            foreach (var item in items)
            {
                itemsDTO.Add(_mapper.Map<DiscountDTO>(item));
            }
         
            return itemsDTO;
        }

        public async Task<List<DiscountDTO>?> GetDiscountsByProductIdAsync(int id)
        {
            var items = await _discountRepository.GetDiscountsByProductIdAsync(id);

            if (!items.Any())
            {
                return null;
            }

            var itemsDTO = new List<DiscountDTO>();
            foreach (var item in items)
            {
                itemsDTO.Add(_mapper.Map<DiscountDTO>(item));
            }

            return itemsDTO;
        }
    }
}
