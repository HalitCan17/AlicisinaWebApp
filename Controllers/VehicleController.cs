using AlicisinaWebApp.Data;
using AlicisinaWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AlicisinaWebApp.Controllers
{
    // Araç ilanlarını listeleme (arama/filtreleme) ve detay görüntüleme işlemlerini yapan Controller
    public class VehicleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Veritabanı ve Kullanıcı servislerini Dependency Injection ile alıyoruz
        public VehicleController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Araçları listeyen ana metot. 
        // id parametresi kategori filtrelemesi, q parametresi arama kelimesi için kullanılır
        public async Task<IActionResult> Index(int? id, string q)
        {
            // Veritabanı sorgusunu oluşturmaya başlıyoruz (Henüz veritabanına gidilmedi - Deferred Execution)
            // Marka (Brand) bilgisini de dahil ediyoruz
            var araclar = _context.Vehicles.Include(v => v.brand).AsQueryable();

            // Eğer kategori ID'si (id) geldiyse, sadece o kategoriye ait markaların araçlarını filtreliyoruz
            if(id != null)
            {
                araclar = araclar.Where(x => x.brand.CategoryId == id);
            }

            // Eğer arama kelimesi (q) geldiyse filtreleme yapıyoruz
            if(!string.IsNullOrEmpty(q))
            {
                string keyword = q.ToLower();
                // Başlıkta, Marka adında veya Model adında aranan kelime geçiyor mu diye kontrol ediyoruz
                araclar = araclar.Where(a => a.Title.ToLower().Contains(keyword) || 
                                             a.brand != null && a.brand.brandName.ToLower().Contains(keyword) || 
                                             a.model != null && a.model.ToLower().Contains(keyword));
            }

            // Oluşturulan sorguyu çalıştırıp listeyi View'a gönderiyoruz
            return View(await araclar.ToListAsync());
        }

        // Seçilen bir aracın detaylarını gösteren metot
        public async Task<IActionResult> Details(int id)
        {
            // İlgili aracı ID'ye göre buluyoruz ve marka bilgisini de dahil ediyoruz
            var vehicle = await _context.Vehicles.Include(v => v.brand).FirstOrDefaultAsync(v => v.vehicleId == id);
            
            // Araç bulunamazsa 404 hatası dönüyoruz
            if(vehicle == null) return NotFound();

            // İlanı veren kullanıcının bilgilerini (Ad, Soyad, Telefon vb.) göstermek için
            // Identity servisinden kullanıcıyı bulup modele ekliyoruz
            if(!string.IsNullOrEmpty(vehicle.AppUserId))
            {
                vehicle.AppUser = await _userManager.FindByIdAsync(vehicle.AppUserId);
            }
            
            return View(vehicle);
        }
    }
}