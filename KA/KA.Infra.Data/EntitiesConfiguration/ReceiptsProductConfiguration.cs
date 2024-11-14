using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class ReceiptsProductConfiguration : IEntityTypeConfiguration<ReceiptProduct>
    {
        public void Configure(EntityTypeBuilder<ReceiptProduct> builder)
        {
            builder.HasKey(rp => new { rp.ReceiptId, rp.ProductId });
            builder.Property(rp => rp.Price).IsRequired();
            builder.Property(rp => rp.Quantity).IsRequired();
            builder.Property(rp => rp.PriceAfterDiscount).IsRequired();
        }
    }
}
