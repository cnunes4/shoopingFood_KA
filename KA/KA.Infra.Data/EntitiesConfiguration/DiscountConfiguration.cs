using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(e => e.DiscountId);
            builder.Property(e => e.Description)
                  .IsRequired()
                  .HasMaxLength(100);
            builder.Property(e => e.Percentage)
                  .IsRequired();
            builder.Property(e => e.IsEnabled)
                .IsRequired();

        }
    }
}
