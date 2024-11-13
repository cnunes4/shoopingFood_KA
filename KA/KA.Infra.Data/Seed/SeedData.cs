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

                //Create Discounts
                context.Discounts.AddRange(
                    new Discount
                    {
                        Id = 1,
                        Description = "10% Discount this week",
                        Percentage = 10,
                        ItemToApply = 4

                    }
                );

                //Create Itens
                context.Products.AddRange(
                    new Product
                    {
                        Id = 1,
                        Name = "Soup",
                        Price = 0.65m
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Bread",
                        Price = 0.80m
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Milk",
                        Price = 1.30m
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Apples",
                        Price = 1.00m
                    }
                );

                // Add promotion
                context.Promotions.AddRange(
                    new Promotion
                    {
                        Id = 1,
                        Description = "Buy 2 soups and get 1 bread for half price",
                        ProductId = 1,
                        QuantityProductId = 2,
                        ProductIdToApply = 2,
                        Percentagem = 50,
                    }
                );


                var user = new User
                {
                    UserId = 1,
                    Username = "kantar",
                    Password = "CqfWpQ/2JYMbLGUQLq9aRQ==$oGpcagdWGN3YO+9yZFoVjriNwwiZ+3TS7MLB6b562o4="
                };

                context.Users.Add(user);
                user = new User
                {
                    UserId = 2,
                    Username = "carla",
                    Password = "iBPUBkIeAGPvjsgfxFFwMw==$jURvQNcSVfxay/HbX0S0GOChW8D8yjwqu1gwyKXnV1s="
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

    }
}
