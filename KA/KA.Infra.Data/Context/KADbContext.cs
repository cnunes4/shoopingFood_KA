
using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KA.Infra.Data.Context
{
    public class KADbContext : DbContext
    {
        public KADbContext(DbContextOptions<KADbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<DiscountProduct> DiscountsProducts { get; set; }

        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<ReceiptProduct> ReceiptProducts { get; set; }
        public virtual DbSet<ReceiptProductDiscount> ReceiptProductDiscounts { get; set; }
        public virtual DbSet<ReceiptProductPromotion> ReceiptProductPromotions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(KADbContext).Assembly);
        }
    }
}
