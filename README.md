🚗 Alicisina Galeri - Araç İlan & Yönetim Sistemi
Bu proje, ASP.NET Core MVC mimarisi kullanılarak geliştirilmiş, kullanıcıların otomobil ve motosiklet ilanları verip inceleyebileceği kapsamlı bir Araç İlan ve Galeri Yönetim Sistemidir.

🎯 Proje Hakkında
Alicisina Galeri, ikinci el veya sıfır araç alım-satım süreçlerini dijitalleştirmeyi hedefler. Kullanıcılar üye olup kendi ilanlarını yönetebilir, detaylı filtreleme seçenekleriyle (Kategori, Marka vb.) aradıkları aracı kolayca bulabilirler.

🌟 Öne Çıkan Özellikler
🔐 Kimlik Doğrulama (Identity): Güvenli üyelik, giriş yapma ve çıkış işlemleri.

📂 Kategori Yönetimi: Otomobil ve Motosiklet gibi farklı araç türlerine göre dinamik listeleme.

🔍 Detaylı Filtreleme: Kategori seçimine göre araçların filtrelenmesi (Örn: Sadece motosikletleri listeleme).

📢 İlan Yönetimi:

Kullanıcılar resimli araç ilanı oluşturabilir.

Sadece kendi ilanlarını düzenleyebilir veya silebilir.

🖼️ Resim Yükleme: İlanlara araç fotoğrafı yükleme desteği.

📱 Responsive Tasarım: Bootstrap ile mobil uyumlu modern arayüz.

🛠️ Teknolojiler
Bu projede aşağıdaki teknolojiler ve kütüphaneler kullanılmıştır:

Backend: ASP.NET Core MVC 10.0

Veritabanı: PostgreSQL

ORM: Entity Framework Core (Code First Yaklaşımı)

Frontend: HTML5, CSS3, Bootstrap 5, JavaScript (jQuery)

Authentication: ASP.NET Core Identity

🗄️ Veritabanı Yapısı
Proje temel olarak aşağıdaki ilişkisel tabloları kullanır:

Vehicles: Araç ilanlarının tutulduğu ana tablo (Fiyat, KM, Model, Resim vb.).

Categories: Araç türleri (Otomobil, Motosiklet).

Brands: Araç markaları (BMW, Mercedes, Yamaha vb.).

AspNetUsers: Kullanıcı ve yetki yönetimi.

🚀 Kurulum
Bu proje SQLite kullandığı için harici bir veritabanı sunucusu kurmanıza gerek yoktur. Veritabanı dosyası proje klasörü içinde otomatik oluşturulur.

Projeyi Klonlayın:


git clone https://github.com/HalitCan17/AlicisinaWebApp.git
cd AlicisinaWebApp

Veritabanını Oluşturun: Visual Studio'da Package Manager Console'u açın ve aşağıdaki komutu çalıştırarak veritabanı dosyasının (AlicisinaWebAppDb.db) oluşmasını sağlayın:


Update-Database

Projeyi Başlatın: Projeyi derleyip çalıştırın (F5 veya dotnet run).


👤 İletişim

LinkedIn: linkedin.com/in/halit-can-18571a353

GitHub: github.com/HalitCan17
