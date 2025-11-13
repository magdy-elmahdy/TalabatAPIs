namespace Talabat.APIs.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}
