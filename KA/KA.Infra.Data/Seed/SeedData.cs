using KA.Domain.Entities;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KA.Infra.Data.Seed
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new KADbContext(serviceProvider.GetRequiredService<DbContextOptions<KADbContext>>()))
            {
                if (context.Discounts.Any() || context.Products.Any() || context.Promotions.Any() || context.Users.Any())
                {
                    return;
                }


                // USERS
                context.Users.AddRange(
                    new User { UserId = 1, Username = "kantar", Password = "CqfWpQ/2JYMbLGUQLq9aRQ==$oGpcagdWGN3YO+9yZFoVjriNwwiZ+3TS7MLB6b562o4=" },
                    new User { UserId = 2, Username = "carla", Password = "iBPUBkIeAGPvjsgfxFFwMw==$jURvQNcSVfxay/HbX0S0GOChW8D8yjwqu1gwyKXnV1s=" }
                );


                //DISCOUNTS
                context.Discounts.AddRange(
                    new Discount { DiscountId = 1, Description = "10% Discount this week", Percentage = 10, IsEnabled=true }
                );


                //Products
                context.Products.AddRange(
                   new Product { IdProduct = 1, Name = "Soup", Price = 0.65m },
                   new Product { IdProduct = 2, Name = "Bread", Price = 0.80m },
                   new Product { IdProduct = 3, Name = "Milk", Price = 1.30m },
                   new Product { IdProduct = 4, Name = "Apples", Price = 1.00m }
                );

                //Promotion
                context.Promotions.AddRange(
                   new Promotion {IdPromotion=1, Description= "Buy 2 soups and get 1 bread for half price",
                   ProductId=1, ProductIdToApply=2, Quantity=2, Percentage=50, DateStart=DateTime.Now, DateEnd= DateTime.Now.AddMonths(2), IsEnabled=true}
                );


                context.SaveChanges();

                //Discount Product
                context.DiscountsProducts.AddRange(
                   new DiscountProduct
                   {
                       DiscountId=1,
                       Discount = context.Discounts.FirstOrDefault(x=> x.DiscountId==1),
                       ProductId=4,
                       Product= context.Products.FirstOrDefault(x => x.IdProduct == 4),
                   } );
                

                context.SaveChanges();
            }
        }

    }
}
