using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasKey(p => p.IdPromotion);
            builder.Property(p => p.Description)
                  .IsRequired()
                  .HasMaxLength(150);
            builder.Property(p => p.ProductIdToApply).IsRequired();
            builder.Property(p => p.ProductId).IsRequired();
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.Percentage).IsRequired();
            builder.Property(p => p.DateStart).IsRequired();
            builder.Property(p => p.DateEnd).IsRequired();
        }
    }
}
