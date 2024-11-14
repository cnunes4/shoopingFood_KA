using KA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KA.Infra.Data.EntitiesConfiguration
{
    internal class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> builder)
        {
            builder.HasKey(r => r.IdReceipt);
            builder.Property(r => r.TotalBeforeDiscount).IsRequired();
            builder.Property(r => r.TotalAfterDiscount).IsRequired();
            builder.Property(r => r.ReceiptDate).IsRequired();
        }
    }
}
