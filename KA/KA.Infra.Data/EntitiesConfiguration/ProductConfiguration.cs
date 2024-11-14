using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.IdProduct);
            builder.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(30);
            builder.Property(e => e.Price)
                  .IsRequired();
        }
    }
}
