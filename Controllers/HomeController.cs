using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlicisinaWebApp.Models;
using AlicisinaWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AlicisinaWebApp.Controllers;

// Uygulamanın ana sayfası ve genel bilgi sayfalarını (Hakkımızda, Yardım vb.) yöneten Controller
public class HomeController : Controller
{
    // Veritabanı erişimi için Context nesnesi
    private readonly AppDbContext _context;

    // Dependency Injection ile veritabanı servisini constructor üzerinden alıyoruz
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    // Ana sayfayı getiren metot
    public IActionResult Index()
    {
        // Veritabanındaki tüm kategorileri listeye çevirip View'a gönderir
        // Ana sayfada kategori listesini göstermek için kullanılır
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    // "Hakkımızda" sayfasını döndüren metot
    public IActionResult About()
    {
        return View();
    }

    // "Kurallar" sayfasını döndüren metot
    public IActionResult Rules()
    {
        return View();
    }

    // "Yardım" sayfasını döndüren metot
    public IActionResult Help()
    {
        return View();
    }

    // Hata oluştuğunda çalışacak metot
    // ResponseCache ile bu sayfanın tarayıcı tarafından önbelleğe alınması engellenir
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // Hata detaylarını içeren ViewModel oluşturulur ve View'a gönderilir
        // RequestId, hatanın takibi (trace) için kullanılır
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}