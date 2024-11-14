using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class DiscountsProductsConfiguration : IEntityTypeConfiguration<DiscountProduct>
    {
        public void Configure(EntityTypeBuilder<DiscountProduct> builder)
        {
            builder.HasKey(e => new { e.DiscountId, e.ProductId });

            builder.HasOne(e => e.Discount)
                  .WithMany(d => d.DiscountProducts)
                  .HasForeignKey(e => e.DiscountId);

            builder.HasOne(e => e.Product)
                  .WithMany(p => p.DiscountProducts)
                  .HasForeignKey(e => e.ProductId);

        }
    }
}
