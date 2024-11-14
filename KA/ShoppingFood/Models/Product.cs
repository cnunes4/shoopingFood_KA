namespace ShoopingFood.Models
{
    public class Product
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public List<Discount> Discounts { get; set; }
    }
}
