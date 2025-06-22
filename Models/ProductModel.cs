namespace SuperMarket.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }


        public override string ToString()
        {
            return $"{nameof(Id)}:{Id}, {nameof(Name)}:{Name}, {nameof(Price)}:{Price}";
        }
    }

}
