namespace Talabat.APIs.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public string BrandNme { get; set; }

        public int BrandId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
