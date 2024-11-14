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
                _logger.LogWarning($"No Products found");
                return null;
            }

            var itemsDTO = await Task.WhenAll(items.Select(async item =>
            {
                var itemDto = _mapper.Map<ProductDTO>(item);
                itemDto.Discounts = this.GetAllDiscountsAsync().Result.Where(x => x.ProductId == item.IdProduct).ToList(); 
                return itemDto;
            }));

            return itemsDTO.ToList();
        }

        /// <summary>
        /// Get one product by Product Id
        /// </summary>
        /// <param name="productId">ProdcutId in DB</param>
        /// <returns>One product with product id </returns>
        public async Task<ProductDTO?> GetProductByIDAsync(int productId)
        {
            var item= await _productRepository.GetProductByIDAsync(productId);

            if (item != null)
            {
                return _mapper.Map<ProductDTO>(item);
            }
            _logger.LogWarning($"No Products found for this id {productId}");
            return null;
           
           
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

    }
}
