using AlicisinaWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlicisinaWebApp.Controllers
{
    // [Authorize] ile bu Controller'a sadece giriş yapmış kullanıcıların erişmesi sağlanır
    [Authorize]
    public class ProfileController : Controller
    {
        // Kullanıcı verilerine erişim ve güncelleme işlemleri için Identity servisi
        private readonly UserManager<ApplicationUser> _userManager;

        // Dependency Injection ile UserManager servisini constructor üzerinden alıyoruz
        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Kullanıcının profil bilgilerini görüntüleyen metot
        public async Task<IActionResult> Index()
        {
            // O anki oturum açmış kullanıcıyı (User) asenkron olarak getiriyoruz
            var user = await _userManager.GetUserAsync(User);
            
            // Kullanıcı bulunamazsa (oturum düşmüş olabilir) hata dönüyoruz
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Profil düzenleme sayfasını getiren GET metodu
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            // Düzenleme formuna mevcut kullanıcı bilgilerini doldurmak için kullanıcıyı getiriyoruz
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Profil güncelleme işlemini yapan POST metodu
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı güvenlik önlemi
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            // Güncellenecek asıl kullanıcı nesnesini veritabanından/Identity'den çekiyoruz
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Formdan (model) gelen yeni verileri, veritabanındaki kullanıcı nesnesine (user) aktarıyoruz
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber; 
            user.BirthDate = model.BirthDate;
            
            // Modelde Phone diye ayrı bir alan varsa onu da güncelliyoruz
            user.Phone = model.Phone;

            // Değişiklikleri Identity mekanizması üzerinden veritabanına kaydediyoruz
            var result = await _userManager.UpdateAsync(user);

            // Güncelleme başarılıysa profil sayfasına yönlendiriyoruz
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa (örn: validasyon hatası), hataları model state'e ekleyip formu tekrar gösteriyoruz
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}