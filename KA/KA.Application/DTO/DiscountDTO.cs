namespace KA.Application.DTO
{
    public class DiscountDTO
    {
        public int Id { get; set; }

        public int ItemToApply { get; set; }

        public string Description { get; set; } = null!;

        public int Percentage { get; set; }
    }
}
