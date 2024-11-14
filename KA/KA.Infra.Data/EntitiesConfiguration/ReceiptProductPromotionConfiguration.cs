using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class ReceiptProductPromotionConfiguration : IEntityTypeConfiguration<ReceiptProductPromotion>
    {
        public void Configure(EntityTypeBuilder<ReceiptProductPromotion> builder)
        {
            builder.HasKey(rpp => new { rpp.ReceiptId, rpp.ProductId, rpp.PromotionId });
        }
    }
}
