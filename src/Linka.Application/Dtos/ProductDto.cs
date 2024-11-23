namespace Linka.Application.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public int AvailableQuantity { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
