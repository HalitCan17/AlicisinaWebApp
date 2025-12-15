using AlicisinaWebApp.Data;
using AlicisinaWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// appsettings.json dosyasından "DefaultConnection" adlı bağlantı cümlesini alıyoruz
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); 

// Uygulamanın ana veritabanı (İlanlar, Kategoriler vb.) için Context ayarı 
builder.Services.AddDbContext<AppDbContext>(options=>
    options.UseSqlite(connectionString)
);

// Kimlik doğrulama sistemi (Identity) için ayrı bir veritabanı Context'i ekleniyor
builder.Services.AddDbContext<AlicisinaIdentityDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection"))
);

// Identity (Kullanıcı Yönetimi) servislerinin yapılandırılması
// Kullanıcı (ApplicationUser) ve Rol (IdentityRole) sınıfları tanımlanıyor
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>() 
    .AddEntityFrameworkStores<AlicisinaIdentityDbContext>() 
    .AddDefaultTokenProviders(); 

var app = builder.Build();

// Geliştirme ortamında değilsek (Canlı ortamdaysak) hata yönetimi ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Hata oluşursa bu sayfaya git
    // HSTS güvenlik başlığı (Tarayıcıyı HTTPS kullanmaya zorlar)
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

// KİMLİK DOĞRULAMA (Authentication): "Sen kimsin?" sorusunu sorar. 
app.UseAuthentication();

// YETKİLENDİRME (Authorization): "Bunu yapmaya iznin var mı?" sorusunu sorar.
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// --- BAŞLANGIÇ VERİLERİNİN YÜKLENMESİ (DATA SEEDING) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Rolleri (Admin, User vb.) ve varsayılan Admin kullanıcısını oluşturmak için yazdığımız metot çağrılıyor
    await RoleSeeder.SeedRolesAndAdminAsync(services);
}


app.Run();