namespace AlicisinaWebApp.Models
{
    public class Brand
    {
        public int brandId { get; set; }
        public int CategoryId { get; set; }
        public string brandName { get; set; }
        public Category category { get; set; }
        public List<Vehicle> vehicles { get; set; }
    }
}