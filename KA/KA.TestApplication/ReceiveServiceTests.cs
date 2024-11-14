using AutoMapper;
using Castle.Core.Logging;
using KA.Application.DTO;
using KA.Application.Services;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace KA.Application.Tests
{
    public class ReceiveServiceTests
    {
        private readonly Mock<IReceiptRepository> _mockReceiptRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<ReceiveService>> _mockServiceLogger;
        private readonly IMapper _mapper;
        private readonly ILogger<ReceiveService> _logger;
        private readonly ReceiveService _service;

        public ReceiveServiceTests()
        {
            _mockReceiptRepository = new Mock<IReceiptRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockServiceLogger = new Mock<ILogger<ReceiveService>>();
            // Setup AutoMapper (using a simple configuration for this example)
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Receiptsproduct, ReceiptItemDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDataDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDTO>().ReverseMap();
            });

            _mapper = configuration.CreateMapper();
            _service = new ReceiveService(_mockReceiptRepository.Object, _mockUserRepository.Object, _mockServiceLogger.Object);
        }

        #region GenerateReceipt Tests

        [Fact]
        public async Task GenerateReceipt_ShouldReturnReceipt_WhenSuccessful()
        {
            // Arrange
            var username = "testuser";
            var basket = new BasketDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO { Id = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { Id = 2, Name = "Bread", Price = 0.80m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>(); // Assume empty discounts
            var promotions = new List<PromotionDTO>(); // Assume empty promotions

            var user = new User { UserId = 1, Username = "testuser" };


            _mockUserRepository.Setup(r => r.GetUserAsync(username)).ReturnsAsync(user);
            _mockReceiptRepository.Setup(r => r.AddReceiptAsync(It.IsAny<Receipt>())).ReturnsAsync(1);  // Simulating successful insertion
            _mockReceiptRepository.Setup(r => r.AddAllProductsToReceiptAsync(It.IsAny<List<Receiptsproduct>>(), 1)).ReturnsAsync(true);

            // Act
            var result = await _service.GenerateReceipt(username, basket, discounts, promotions);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2.10m, result.TotalAfterDiscount);
            Assert.Equal(2.10m, result.TotalBeforeDiscount);
            Assert.Equal(0, result.TotalDiscount);
        }


        [Fact]
        public async Task GenerateReceipt_ShouldReturnReceipt_WithDiscounts()
        {
            // Arrange
            var username = "testuser";
            var basket = new BasketDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO { Id = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { Id = 2, Name = "Bread", Price = 0.80m, Quantity = 1 },
                    new ProductDTO { Id = 3, Name = "Milk", Price = 1.30m, Quantity = 0 },
                    new ProductDTO { Id = 4, Name = "Apples", Price = 1.00m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>() {
                new DiscountDTO {  Id=1, Description="Apply with 10%", ItemToApply=4, Percentage=10 }
            };

            var promotions = new List<PromotionDTO>(); // Assume empty promotions

            var user = new User { UserId = 1, Username = "testuser" };


            _mockUserRepository.Setup(r => r.GetUserAsync(username)).ReturnsAsync(user);
            _mockReceiptRepository.Setup(r => r.AddReceiptAsync(It.IsAny<Receipt>())).ReturnsAsync(1);  // Simulating successful insertion
            _mockReceiptRepository.Setup(r => r.AddAllProductsToReceiptAsync(It.IsAny<List<Receiptsproduct>>(), 1)).ReturnsAsync(true);

            // Act
            var result = await _service.GenerateReceipt(username, basket, discounts, promotions);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3.00m, result.TotalAfterDiscount);
            Assert.Equal(3.10m, result.TotalBeforeDiscount);
            Assert.Equal(0.10m, result.TotalDiscount);
        }


        [Fact]
        public async Task GenerateReceipt_ShouldReturnReceipt_WithDiscountsAndPromotions()
        {
            // Arrange
            var username = "testuser";
            var basket = new BasketDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO { Id = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { Id = 2, Name = "Bread", Price = 0.80m, Quantity = 1 },
                    new ProductDTO { Id = 3, Name = "Milk", Price = 1.30m, Quantity = 0 },
                    new ProductDTO { Id = 4, Name = "Apples", Price = 1.00m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>() {
                new DiscountDTO {  Id=1, Description="Apply with 10%", ItemToApply=4, Percentage=10 }
            };

            var promotions = new List<PromotionDTO>() {
                new PromotionDTO {  Id=1, Description="Buy 2 soup and receive a bread with half price", Percentagem=50, ProductId=1, ProductIdToApply=2, QuantityProductId=2 }
            };

            var user = new User { UserId = 1, Username = "testuser" };


            _mockUserRepository.Setup(r => r.GetUserAsync(username)).ReturnsAsync(user);
            _mockReceiptRepository.Setup(r => r.AddReceiptAsync(It.IsAny<Receipt>())).ReturnsAsync(1);  // Simulating successful insertion
            _mockReceiptRepository.Setup(r => r.AddAllProductsToReceiptAsync(It.IsAny<List<Receiptsproduct>>(), 1)).ReturnsAsync(true);

            // Act
            var result = await _service.GenerateReceipt(username, basket, discounts, promotions);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2.60m, result.TotalAfterDiscount);
            Assert.Equal(3.10m, result.TotalBeforeDiscount);
            Assert.Equal(0.50m, result.TotalDiscount);
        }
        #endregion
    }
}