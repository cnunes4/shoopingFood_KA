namespace KA.Application.DTO
{
    public class PromotionDTO
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public int ProductIdToApply { get; set; }

        public int ProductId { get; set; }

        public int QuantityProductId { get; set; }

        public decimal Percentagem { get; set; }
    }
}
