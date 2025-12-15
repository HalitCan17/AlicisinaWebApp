using AlicisinaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AlicisinaWebApp.Data
{
    // Veritabanı ile uygulama arasındaki köprü görevi gören Context sınıfı
    public class AppDbContext : DbContext
    {
        // Yapılandırıcı metot (Constructor)
        // Veritabanı bağlantı ayarlarını (Connection String vb.) Base sınıfa (DbContext) iletir
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veritabanındaki tabloların C# tarafındaki karşılıkları (DbSet)
        public DbSet<Vehicle> Vehicles { get; set; }  // Araçlar tablosu
        public DbSet<Brand> Brands { get; set; }      // Markalar tablosu
        public DbSet<Category> Categories { get; set; } // Kategoriler tablosu

        // Model oluşturulurken çalışacak konfigürasyonlar ve varsayılan veriler (Seeding)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Veritabanı ilk oluştuğunda eklenecek varsayılan Kategoriler
            modelBuilder.Entity<Category>().HasData(
                new Category { categoryId = 1, categoryName = "Otomobil"},
                new Category { categoryId = 2, categoryName = "Motorsiklet"}
            );

            // Veritabanı ilk oluştuğunda eklenecek varsayılan Markalar
            modelBuilder.Entity<Brand>().HasData(
                new Brand { brandId = 1 , brandName = "BMW" , CategoryId = 1 },
                new Brand { brandId = 2 , brandName = "Mercedes" , CategoryId = 1 },
                new Brand { brandId = 3 , brandName = "Yamaha" , CategoryId = 2 },
                new Brand { brandId = 4 , brandName = "Suzuki" , CategoryId = 2 },
                new Brand { brandId = 5 , brandName = "Honda" , CategoryId = 2 },
                new Brand { brandId = 6 , brandName = "Mondial" , CategoryId = 2 },
                new Brand { brandId = 7 , brandName = "Kawasaki" , CategoryId = 2 }
            );

            // Veritabanı ilk oluştuğunda eklenecek varsayılan Araç İlanları
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    vehicleId = 1,
                    Title = "Sahibinden Temiz 3.20i SportLine",
                    announcementDate = new DateTime(2025,12,3),
                    model = "3.20i Sport Line",
                    price = 2850000,
                    kilometer = 56000,
                    color = "Mavi",
                    year = 2022,
                    serie = "3 Serisi",
                    fuelType = "Benzinli",
                    gear = "Otomatik",
                    bodyType = "Sedan",
                    enginePower = "170hp",
                    engineDisplacement = "1597cc",
                    vehicleStatus = "İkinci el",
                    imageUrl = "/images/320i.jpg",
                    brandId = 1,
                    Description = "BLA BLA BLA",
                    categoryId = 1,
                    AppUserId = "3b2e3887-72a6-440a-b0f1-c50fa6322d9f"  
                },
                new Vehicle
                {
                    vehicleId = 2,
                    Title = "Boyasız Tramersiz E180 Exclusive",
                    announcementDate = new DateTime(2025,12,1),
                    model = "E180 Exclusive",
                    price = 3250000,
                    kilometer = 100000,
                    color = "Black",
                    year = 2017,
                    serie = "E Serisi",
                    fuelType = "Benzinli",
                    gear = "Otomatik",
                    bodyType = "Sedan",
                    enginePower = "156hp",
                    engineDisplacement = "1595cc",
                    vehicleStatus = "İkinci el",
                    imageUrl = "/images/e180.jpg",
                    brandId = 2,
                    Description = "BLA BLA BLA",
                    categoryId = 1,
                    AppUserId = "3b2e3887-72a6-440a-b0f1-c50fa6322d9f"
                },
                new Vehicle
                {
                    vehicleId = 3,
                    Title = "Dosta Gider R25",
                    announcementDate = new DateTime(2025,11,19),
                    model = "YZF R25 ABS",
                    price = 210000,
                    kilometer = 15000,
                    color = "Red",
                    year = 2021,
                    serie = "-",
                    fuelType = "Benzinli",
                    gear = "Manuel",
                    bodyType = "Super Sport",
                    enginePower = "25hp",
                    engineDisplacement = "250cc",
                    vehicleStatus = "İkinci el",
                    imageUrl = "/images/r25.jpeg",
                    brandId = 3,
                    Description = "BLA BLA BLA",
                    categoryId = 2,
                    AppUserId = "3b2e3887-72a6-440a-b0f1-c50fa6322d9f"
                }
            );
        }
    }
}