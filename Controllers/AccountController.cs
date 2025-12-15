using AlicisinaWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlicisinaWebApp.Controllers
{
    // Bu controller kullanıcı işlemleri (register, login, logout) için kullanılır
    public class AccountController : Controller
    {
        // Identity yapısından gelen kullanıcı yönetimi için gerekli servisler
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Dependency Injection ile UserManager ve SignInManager sınıflarını alıyoruz
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Kayıt olma sayfasını ekrana getiren GET metodu
        [HttpGet]
        public IActionResult Register()
        {
            // Sadece View döndürülür, herhangi bir işlem yapılmaz
            return View();
        }

        // Kayıt olma formu gönderildiğinde çalışan POST metodu
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Model doğrulaması başarısızsa form tekrar gösterilir
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Yeni bir kullanıcı nesnesi oluşturulur
            var user = new ApplicationUser();

            // Formdan gelen bilgiler kullanıcı nesnesine aktarılır
            user.UserName = model.Email;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BirthDate = model.BirthDate;
            user.Phone = model.Phone;

            // Kullanıcı veritabanına eklenir
            var result = await _userManager.CreateAsync(user, model.Password!);

            // Eğer kayıt işlemi başarılıysa login sayfasına yönlendirilir
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            // Hata varsa Identity tarafından dönen hatalar ekrana basılır
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            // Hatalı durumda form tekrar gösterilir
            return View(model);
        }

        // Login sayfasını ekrana getiren GET metodu
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login formu gönderildiğinde çalışan POST metodu
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Model doğrulaması kontrol edilir
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Email ve şifre ile giriş yapılmaya çalışılır
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                false,  // remember me aktif değil
                false); // başarısız girişte kilitleme yok

            // Giriş başarılıysa ana sayfaya yönlendirilir
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // Giriş başarısızsa hata mesajı gösterilir
            ModelState.AddModelError("", "Email veya şifre yanlış.");
            return View(model);
        }

        // Kullanıcının sistemden çıkış yapmasını sağlar
        public async Task<IActionResult> Logout()
        {
            // Oturum kapatılır
            await _signInManager.SignOutAsync();

            // Login sayfasına yönlendirilir
            return RedirectToAction("Login");
        }
    }
}
