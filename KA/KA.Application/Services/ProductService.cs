using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;

namespace KA.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                cfg.CreateMap<Discount, DiscountDTO>().ReverseMap();
            }).CreateMapper();
        }

        public async Task<List<ProductDTO>?> GetAllProductsAsync()
        {
             var items = await _productRepository.GetAllProductsAsync();

            if (!items.Any())
            {
                return null;
            }


            var itemsDTO = new List<ProductDTO>();
            foreach (var item in items)
            {
                var itemDto = _mapper.Map<ProductDTO>(item);
                itemDto.Discounts = GetAllDiscounts(item.Id);
                itemsDTO.Add(itemDto);
            }

            return itemsDTO;
        }

        public async Task<ProductDTO> GetItemByIDAsync(int id)
        {
            var item= await _productRepository.GetItemByIDAsync(id);
            return _mapper.Map<ProductDTO>(item);
           
        }

        private List<DiscountDTO> GetAllDiscounts(int idProduct)
        {
            var listOfDiscounts = new List<DiscountDTO>();
            var discounts = _discountRepository.GetDiscountsByProductIdAsync(idProduct);

            if (discounts.Result != null)
            {
                foreach (var item in discounts.Result)
                {
                    listOfDiscounts.Add(_mapper.Map<DiscountDTO>(item));
                }

                return listOfDiscounts;
            }

            return null;
            
        }
    }
}
