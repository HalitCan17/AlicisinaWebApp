using System.ComponentModel.DataAnnotations.Schema;

namespace AlicisinaWebApp.Models
{
    public class Vehicle
    {
        public int vehicleId { get; set; }
        public string Title { get; set; }
        public DateTime announcementDate { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public int year { get; set; }
        public string serie { get; set; }
        public string fuelType { get; set; }
        public string gear { get; set; }
        public decimal kilometer { get; set; }
        public string bodyType { get; set; }
        public string enginePower { get; set; }
        public string engineDisplacement { get; set; }
        public decimal price { get; set; }
        public string imageUrl { get; set; }
        public string vehicleStatus { get; set; }
        public int brandId { get; set; }
        public Brand brand { get; set; }
        public string? Description { get; set; }
        public string? AppUserId { get; set; }
        // [ForeignKey("AppUserId")]
        [NotMapped]
        public ApplicationUser AppUser { get; set; }
        public int? categoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public bool IsRulesAccepted { get; set; }
    }
}