using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class ReceiptsProductConfiguration : IEntityTypeConfiguration<Receiptsproduct>
    {
        public void Configure(EntityTypeBuilder<Receiptsproduct> builder)
        {
            builder.HasKey(rp => new { rp.ReceiptId, rp.ProductId });
        }
    }
}
