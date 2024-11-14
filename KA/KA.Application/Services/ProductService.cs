using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KA.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository productRepository, IDiscountRepository discountRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _logger = logger;
            // This configuration sets up a bidirectional mapping between the Product and ProductDTO classes.
            // and between the Discount and DiscountDTO classes.
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                cfg.CreateMap<Discount, DiscountDTO>().ReverseMap();
            }).CreateMapper();
        }
        /// <summary>
        /// Get all products in BD
        /// </summary>
        /// <returns>List of products </returns>
        public async Task<List<ProductDTO>?> GetAllProductsAsync()
        {
            var items = await _productRepository.GetAllProductsAsync();

            if (items == null || !items.Any())
            {
                _logger.LogError($"No Products found");
                return null;
            }

            var itemsDTO = await Task.WhenAll(items.Select(async item =>
            {
                var itemDto = _mapper.Map<ProductDTO>(item);
                itemDto.Discounts = await GetAllDiscounts(item.Id); 
                return itemDto;
            }));

            return itemsDTO.ToList();
        }

        /// <summary>
        /// Get one product by Product Id
        /// </summary>
        /// <param name="id">ProdcutId in DB</param>
        /// <returns>One product with product id </returns>
        public async Task<ProductDTO?> GetItemByIDAsync(int id)
        {
            var item= await _productRepository.GetItemByIDAsync(id);

            if (item != null)
            {
                return _mapper.Map<ProductDTO>(item);
            }
            _logger.LogError($"No Products found for this id {id}");
            return null;
           
           
        }

        /// <summary>
        /// Gel all discounts for one product
        /// </summary>
        /// <param name="idProduct">ProdcutId in DB</param>
        /// <returns>List of all discount for this product id </returns>
        private async Task<List<DiscountDTO>?> GetAllDiscounts(int idProduct)
        {
            var discounts = await _discountRepository.GetDiscountsByProductIdAsync(idProduct);

            if (discounts != null)
            {
                return discounts.Select(item => _mapper.Map<DiscountDTO>(item)).ToList();
            }
            _logger.LogError($"No Discounts found for this id {idProduct}");
            return null;
        }
    }
}
