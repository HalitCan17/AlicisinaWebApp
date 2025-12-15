namespace AlicisinaWebApp.Models
{
    public class Category
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public List<Brand> brand { get; set; }
    }
}