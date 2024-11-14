using AutoMapper;
using Castle.Core.Logging;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Application.Services;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace KA.Application.Tests
{
    public class ReceiveServiceTests
    {
        private readonly Mock<IReceiptRepository> _receiptRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly Mock<IPromotionService> _promotionServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ILogger<ReceiveService>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly ReceiveService _receiveService;


        public ReceiveServiceTests()
        {
            _receiptRepositoryMock = new Mock<IReceiptRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _discountServiceMock = new Mock<IDiscountService>();
            _promotionServiceMock = new Mock<IPromotionService>();
            _productServiceMock = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger<ReceiveService>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptProduct, ReceiptItemDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDataDTO>().ReverseMap();
                cfg.CreateMap<Receipt, ReceiptDTO>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _receiveService = new ReceiveService(
                _receiptRepositoryMock.Object,
                _userRepositoryMock.Object,
                _loggerMock.Object,
                _discountServiceMock.Object,
                _promotionServiceMock.Object,
                _productServiceMock.Object
            );
        }

        #region GenerateReceipt Tests



        [Fact]
        public async Task CalculateReceiptAsync_ShouldCalculateReceiptTotalsCorrectly()
        {
            // Arrange
            var basket = new BasketDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO { IdProduct = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { IdProduct = 2, Name = "Bread", Price = 0.80m, Quantity = 1 },
                    new ProductDTO { IdProduct = 3, Name = "Milk", Price = 1.30m, Quantity = 0 },
                    new ProductDTO { IdProduct = 4, Name = "Apples", Price = 1.00m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>(); // Assume empty discounts
            var promotions = new List<PromotionDTO>(); // Assume empty promotions

            var product = new ProductDTO ();


            _promotionServiceMock.Setup(p => p.GetAllPromotionsAsync()).ReturnsAsync(new List<PromotionDTO>());
            _discountServiceMock.Setup(d => d.GetDiscountsByProductIdAsync(It.IsAny<int>())).ReturnsAsync(new List<DiscountDTO>());
            _productServiceMock.Setup(p => p.GetProductByIDAsync(It.IsAny<int>())).ReturnsAsync(new ProductDTO { Name = "Product" });

            // Act
            var result = await _receiveService.CalculateReceiptAsync(basket, promotions, discounts);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3.10m, result.TotalAfterDiscount);
            Assert.Equal(3.10m, result.TotalBeforeDiscount);
            Assert.Equal(0, result.TotalDiscount);
        }


        [Fact]
        public async Task GenerateReceipt_ShouldReturnReceiptData_WhenReceiptGeneratedSuccessfully()
        {
            // Arrange
            var basket = new BasketDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO { IdProduct = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { IdProduct = 2, Name = "Bread", Price = 0.80m, Quantity = 1 },
                    new ProductDTO { IdProduct = 3, Name = "Milk", Price = 1.30m, Quantity = 0 },
                    new ProductDTO { IdProduct = 4, Name = "Apples", Price = 1.00m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>(); // Assume empty discounts
            var promotions = new List<PromotionDTO>(); // Assume empty promotions

            var product = new ProductDTO();


            var user = new User { UserId = 1, Username = "testUser" };

            _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            _receiptRepositoryMock.Setup(r => r.AddReceiptAsync(It.IsAny<Receipt>())).ReturnsAsync(1);
            _receiptRepositoryMock.Setup(r => r.AddAllProductsToReceiptAsync(It.IsAny<List<ReceiptProduct>>(), It.IsAny<int>())).ReturnsAsync(true);
            _promotionServiceMock.Setup(p => p.GetAllPromotionsAsync()).ReturnsAsync(new List<PromotionDTO>());
            _discountServiceMock.Setup(d => d.GetDiscountsByProductIdAsync(It.IsAny<int>())).ReturnsAsync(new List<DiscountDTO>());
            _productServiceMock.Setup(p => p.GetProductByIDAsync(It.IsAny<int>())).ReturnsAsync(new ProductDTO { Name = "Product" });

            // Act
            var result = await _receiveService.GenerateReceipt("testUser", basket, promotions, discounts);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3.10m, result.TotalAfterDiscount);
            Assert.Equal(3.10m, result.TotalBeforeDiscount);
            Assert.Equal(0, result.TotalDiscount);
        }

       
        [Fact]
        public async Task GetAllReceivesByUserId_ShouldReturnReceipts_WhenUserExists()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testUser" };
            var receipts = new List<Receipt>
            {
                new Receipt { TotalBeforeDiscount = 50, TotalAfterDiscount = 40, ReceiptDate = DateTime.Now }
            };
            _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            _receiptRepositoryMock.Setup(r => r.GetAllReceiptsByUserAsync(It.IsAny<int>())).ReturnsAsync(receipts);

            // Act
            var result = await _receiveService.GetAllReceivesByUserId("testUser");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(50, result[0].TotalBeforeDiscount);
            Assert.Equal(40, result[0].TotalAfterDiscount);
            Assert.Equal(10, result[0].TotalDiscount);
        }

        [Fact]
        public async Task GetReceiptByReceiptId_ShouldReturnReceipt_WhenReceiptExists()
        {
            // Arrange
            var receipt = new Receipt { TotalBeforeDiscount = 30, TotalAfterDiscount = 25 };
            _receiptRepositoryMock.Setup(r => r.GetReceiptAsync(It.IsAny<int>())).ReturnsAsync(receipt);

            // Act
            var result = await _receiveService.GetReceiptByReceiptId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(30, result.TotalBeforeDiscount);
            Assert.Equal(25, result.TotalAfterDiscount);
            Assert.Equal(5, result.TotalDiscount);
        }


        [Fact]
        public async Task GenerateReceipt_ShouldReturnReceipt_WhenSuccessful()
        {
            // Arrange
            var basket = new BasketDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO { IdProduct = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { IdProduct = 2, Name = "Bread", Price = 0.80m, Quantity = 1 },
                    new ProductDTO { IdProduct = 3, Name = "Milk", Price = 1.30m, Quantity = 0 },
                    new ProductDTO { IdProduct = 4, Name = "Apples", Price = 1.00m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>(); // Assume empty discounts
            var promotions = new List<PromotionDTO>(); // Assume empty promotions

            var product = new ProductDTO();


            var user = new User { UserId = 1, Username = "testUser" };

            _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            _receiptRepositoryMock.Setup(r => r.AddReceiptAsync(It.IsAny<Receipt>())).ReturnsAsync(1);
            _receiptRepositoryMock.Setup(r => r.AddAllProductsToReceiptAsync(It.IsAny<List<ReceiptProduct>>(), It.IsAny<int>())).ReturnsAsync(true);
            _promotionServiceMock.Setup(p => p.GetAllPromotionsAsync()).ReturnsAsync(new List<PromotionDTO>());
            _discountServiceMock.Setup(d => d.GetDiscountsByProductIdAsync(It.IsAny<int>())).ReturnsAsync(new List<DiscountDTO>());
            _productServiceMock.Setup(p => p.GetProductByIDAsync(It.IsAny<int>())).ReturnsAsync(new ProductDTO { Name = "Product" });


            // Act
            var result = await _receiveService.GenerateReceipt(user.Username, basket, promotions, discounts);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3.10m, result.TotalAfterDiscount);
            Assert.Equal(3.10m, result.TotalBeforeDiscount);
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
                    new ProductDTO { IdProduct = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { IdProduct = 2, Name = "Bread", Price = 0.80m, Quantity = 1 },
                    new ProductDTO { IdProduct = 3, Name = "Milk", Price = 1.30m, Quantity = 0 },
                    new ProductDTO { IdProduct = 4, Name = "Apples", Price = 1.00m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>() {
                new DiscountDTO {  DiscountId=1, Description="Apply with 10%",  Percentage=10, ProductId=4 }
            };

            var product = new ProductDTO();
            var promotions = new List<PromotionDTO>(); // Assume empty promotions
            var user = new User { UserId = 1, Username = "testUser" };

            _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            _receiptRepositoryMock.Setup(r => r.AddReceiptAsync(It.IsAny<Receipt>())).ReturnsAsync(1);
            _receiptRepositoryMock.Setup(r => r.AddAllProductsToReceiptAsync(It.IsAny<List<ReceiptProduct>>(), It.IsAny<int>())).ReturnsAsync(true);
            _promotionServiceMock.Setup(p => p.GetAllPromotionsAsync()).ReturnsAsync(new List<PromotionDTO>());
            _discountServiceMock.Setup(d => d.GetDiscountsByProductIdAsync(It.IsAny<int>())).ReturnsAsync(discounts);
            _productServiceMock.Setup(p => p.GetProductByIDAsync(It.IsAny<int>())).ReturnsAsync(new ProductDTO { IdProduct = 4, Name = "Apples", Price = 1.00m, Quantity = 1 });
            // Act
            var result = await _receiveService.GenerateReceipt(user.Username, basket, promotions, discounts);

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
                    new ProductDTO { IdProduct = 1, Name = "Soup", Price = 0.65m, Quantity = 2 },
                    new ProductDTO { IdProduct = 2, Name = "Bread", Price = 0.80m, Quantity = 1 },
                    new ProductDTO { IdProduct = 3, Name = "Milk", Price = 1.30m, Quantity = 0 },
                    new ProductDTO { IdProduct = 4, Name = "Apples", Price = 1.00m, Quantity = 1 }
                }
            };

            var discounts = new List<DiscountDTO>() {
                new DiscountDTO {  DiscountId=1, Description="Apply with 10%", Percentage=10, ProductId=4 }
            };

            var promotions = new List<PromotionDTO>() {
                new PromotionDTO {  IdPromotion=1, Description="Buy 2 soup and receive a bread with half price", Percentage=50, ProductId=1, ProductIdToApply=2, Quantity=2 }
            };

            var user = new User { UserId = 1, Username = "testUser" };

            _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            _receiptRepositoryMock.Setup(r => r.AddReceiptAsync(It.IsAny<Receipt>())).ReturnsAsync(1);
            _receiptRepositoryMock.Setup(r => r.AddAllProductsToReceiptAsync(It.IsAny<List<ReceiptProduct>>(), It.IsAny<int>())).ReturnsAsync(true);
            _promotionServiceMock.Setup(p => p.GetAllPromotionsAsync()).ReturnsAsync(promotions);
            _discountServiceMock.Setup(d => d.GetDiscountsByProductIdAsync(It.IsAny<int>())).ReturnsAsync(discounts);
            _productServiceMock.Setup(p => p.GetProductByIDAsync(It.IsAny<int>())).ReturnsAsync(new ProductDTO { Name = "Product" });

            // Act
            var result = await _receiveService.GenerateReceipt(user.Username, basket, promotions, discounts);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2.60m, result.TotalAfterDiscount);
            Assert.Equal(3.10m, result.TotalBeforeDiscount);
            Assert.Equal(0.50m, result.TotalDiscount);
        }
        #endregion
    }
}