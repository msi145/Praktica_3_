namespace WmsClient.Models
{
    // Единственная модель, которую мы сериализуем и шлём в API/1С
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = "";
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public string Zone { get; set; } = "";
    }
}
