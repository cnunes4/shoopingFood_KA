using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class ReceiptProductDiscountConfiguration : IEntityTypeConfiguration<ReceiptProductDiscount>
    {
        public void Configure(EntityTypeBuilder<ReceiptProductDiscount> builder)
        {
            builder.HasKey(rpd => new { rpd.ReceiptId, rpd.ProductId, rpd.DiscountId });


        }
    }
}
