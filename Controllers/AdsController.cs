using AlicisinaWebApp.Data;
using AlicisinaWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlicisinaWebApp.Controllers
{
    // [Authorize] attribute'u sayesinde bu Controller'a sadece giriş yapmış kullanıcılar erişebilir
    [Authorize]
    public class AdsController : Controller
    {
        // Veritabanı işlemleri, kullanıcı yönetimi ve dosya işlemleri için gerekli servisler
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        // Dependency Injection (Bağımlılık Enjeksiyonu) ile servisleri constructor üzerinden alıyoruz
        public AdsController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        // Kullanıcının kendi eklediği ilanları listeyen metot
        public async Task<IActionResult> Index()
        {
            // O an sisteme giriş yapmış kullanıcının ID'sini alıyoruz
            var userId = _userManager.GetUserId(User);

            // Veritabanından sadece bu kullanıcıya ait araçları getiriyoruz
            // Include ile ilişkili marka (Brand) verisini de dahil ediyoruz (Eager Loading)
            var userAds = await _context.Vehicles
                                        .Include(v => v.brand)
                                        .Where(x => x.AppUserId == userId) // Filtreleme: Sadece kendi ilanları
                                        .OrderByDescending(x => x.announcementDate) // Tarihe göre yeniden eskiye
                                        .ToListAsync();
            return View(userAds);
        }

        // Seçilen ilanın detaylarını gösteren metot
        public async Task<IActionResult> Details(int? id)
        {
            // ID gelmediyse hata döner
            if (id == null) return NotFound();

            // İlan verisini marka bilgisiyle beraber veritabanından çekiyoruz
            var vehicle = await _context.Vehicles
                .Include(v => v.brand)
                .FirstOrDefaultAsync(m => m.vehicleId == id);

            // Araç bulunamadıysa hata döner
            if (vehicle == null) return NotFound();

            // İlan sahibinin (User) detaylarını sayfada göstermek için ayrıca yüklüyoruz
            if (!string.IsNullOrEmpty(vehicle.AppUserId))
            {
                vehicle.AppUser = await _userManager.FindByIdAsync(vehicle.AppUserId);
            }

            return View(vehicle);
        }

        // Yeni ilan ekleme sayfasını getiren GET metodu
        [HttpGet]
        public IActionResult Create()
        {
            // Kategori ve Marka seçimleri için Dropdown (SelectList) verilerini ViewBag'e yüklüyoruz
            ViewBag.Categories = new SelectList(_context.Categories, "categoryId", "categoryName");
            ViewBag.Brands = new SelectList(_context.Brands, "brandId", "brandName");

            return View();
        }

        // İlan formu gönderildiğinde çalışan POST metodu (Resim yükleme işlemini de içerir)
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Create(Vehicle vehicle, IFormFile? imageFile)
        {
            // İlanı ekleyen kullanıcının ID'sini modele atıyoruz
            var userId = _userManager.GetUserId(User);
            vehicle.AppUserId = userId;
            
            // İlan tarihi ve varsayılan durum atanıyor
            vehicle.announcementDate = DateTime.Now;
            vehicle.vehicleStatus = "Yayında";

            // Seri bilgisi girilmediyse varsayılan değer atanıyor
            if (string.IsNullOrEmpty(vehicle.serie))
            {
                vehicle.serie = "Belirtilmemiş";
            }

            // Resim yükleme işlemi kontrolü
            if (imageFile != null)
            {
                // Resmin yükleneceği klasör yolu: wwwroot/images
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                
                // Dosya adı çakışmasını önlemek için GUID ile benzersiz bir isim oluşturuluyor
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    // Dosya fiziksel olarak sunucuya kopyalanıyor
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    
                    // Veritabanına kaydedilecek dosya yolu set ediliyor
                    vehicle.imageUrl = "/images/" + uniqueFileName;

                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // Veritabanı kaydında hata olursa, yüklenen resmi sunucudan siliyoruz (Cleanup)
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    throw;
                }
            }
            else
            {
                // Kullanıcı resim yüklemediyse varsayılan araç resmi atanıyor
                vehicle.imageUrl = "/images/default-car.jpg";
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // İlan düzenleme sayfasını getiren GET metodu
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            
            // Sadece ilanın sahibi olan kullanıcı bu ilanı düzenleyebilir kontrolü
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.vehicleId == id && x.AppUserId == userId);

            if (vehicle == null)
            {
                return NotFound();
            }

            // Marka seçimi için mevcut markayı seçili getirerek Dropdown dolduruyoruz
            ViewBag.Brands = new SelectList(_context.Brands, "brandId", "brandName", vehicle.brandId);

            return View(vehicle);
        }

        // İlan güncelleme işlemini yapan POST metodu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle, IFormFile? imageFile)
        {
            // ID uyuşmazlığı kontrolü
            if (id != vehicle.vehicleId) return NotFound();

            var userId = _userManager.GetUserId(User);
            
            // Güncellenecek veriyi veritabanından çekiyoruz (Yine sahiplik kontrolü ile)
            var existingVehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.vehicleId == id && x.AppUserId == userId);

            if (existingVehicle == null) return NotFound();

            try
            {
                // Formdan gelen güncel bilgileri mevcut nesneye aktarıyoruz
                existingVehicle.Title = vehicle.Title;
                existingVehicle.brandId = vehicle.brandId;
                existingVehicle.model = vehicle.model;
                existingVehicle.price = vehicle.price;
                existingVehicle.kilometer = vehicle.kilometer;
                existingVehicle.year = vehicle.year;
                existingVehicle.color = vehicle.color;
                existingVehicle.gear = vehicle.gear;
                existingVehicle.fuelType = vehicle.fuelType;
                existingVehicle.bodyType = vehicle.bodyType;
                existingVehicle.enginePower = vehicle.enginePower;
                existingVehicle.engineDisplacement = vehicle.engineDisplacement;
                existingVehicle.serie = vehicle.serie;
                existingVehicle.Description = vehicle.Description;

                // Eğer yeni bir resim yüklendiyse
                if (imageFile != null)
                {
                    // Eski resim varsayılan resim değilse sunucudan siliyoruz
                    if (!string.IsNullOrEmpty(existingVehicle.imageUrl) && !existingVehicle.imageUrl.Contains("default-car.jpg"))
                    {
                        var oldPath = Path.Combine(_hostEnvironment.WebRootPath, existingVehicle.imageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    // Yeni resmi kaydediyoruz
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    
                    // Veritabanındaki resim yolunu güncelliyoruz
                    existingVehicle.imageUrl = "/images/" + uniqueFileName;
                }
                
                // Değişiklikleri veritabanına yansıtıyoruz
                _context.Update(existingVehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // İlan silme işlemini yapan metot
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            
            // Silinecek aracı buluyoruz (Yine sadece sahibi silebilir)
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.vehicleId == id && x.AppUserId == userId);

            if (vehicle != null)
            {
                // Eğer aracın resmi varsa ve varsayılan resim değilse, sunucudan dosyasını da siliyoruz
                if (vehicle.imageUrl != null && !vehicle.imageUrl.Contains("default-car.jpg"))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, vehicle.imageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Kaydı veritabanından siliyoruz
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}