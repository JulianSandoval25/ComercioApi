namespace ComercioApi.Models
{
    public class Product
    {
        public int id { get; set; }
        public required string name { get; set; }
        public required string description { get; set; }
        public required decimal price { get; set; }
    }
}
