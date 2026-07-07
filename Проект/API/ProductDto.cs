namespace WmsApi.Models
{
    // Должна совпадать по составу полей с моделью Product на стороне WPF
    public class ProductDto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = "";
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public string Zone { get; set; } = "";
    }
}
