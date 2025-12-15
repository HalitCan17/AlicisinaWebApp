using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlicisinaWebApp.Models;

namespace AlicisinaWebApp.Data
{
    // Bu sınıf uygulamanın Identity (kullanıcı) veritabanı işlemlerini yönetir
    // IdentityDbContext'ten türediği için kullanıcı tabloları otomatik oluşur
    public class AlicisinaIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        // DbContext ayarları Program.cs üzerinden buraya gönderilir
        public AlicisinaIdentityDbContext(DbContextOptions<AlicisinaIdentityDbContext> options)
            : base(options)
        {
            // Ek bir ayar yapılmadığı için constructor boş bırakılmıştır
        }
    }
}
