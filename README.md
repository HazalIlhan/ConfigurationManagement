# Configuration Management Project

Bu proje, uygulamalar için dinamik bir konfigürasyon yönetim sistemi sağlar.  
ConfigurationReader kütüphanesi sayesinde uygulamalar, veritabanındaki konfigürasyon kayıtlarını runtime sırasında çekebilir, güncelleyebilir ve sadece aktif kayıtları kullanabilir.  
Web arayüzü ile CRUD işlemleri ve filtreleme yapılabilir.

## Özellikler
- Dinamik konfigürasyon yönetimi (int, string, bool, double tiplerinde)
- Sadece `IsActive = true` olan kayıtları döndürür
- Web UI üzerinden CRUD işlemleri ve filtreleme
- Deployment veya restart gerektirmeden yeni kayıtları okuyabilir
- Docker ile kolayca çalıştırılabilir

## Proje Yapısı
- **ConfigurationManagement.Library** → Konfigürasyon okuma kütüphanesi  
- **ConfigurationManagement.DAL** → Entity Framework Core DbContext ve Entity tanımları  
- **ConfigurationManagement.Models** → ViewModel tanımları  
- **ConfigurationManagement** → ASP.NET Core MVC uygulaması ve Razor Pages  

---

## Başlangıç

### Gereksinimler
- .NET 8 
- Docker & Docker Compose  

### Local Çalıştırma
Repository’i klonlayın:
```bash
git clone https://github.com/HazalIlhan/ConfigurationManagement.git
cd ConfigurationManagement/ConfigurationManagement

Database migrationları uygulayın:

dotnet ef database update


Uygulamayı başlatın:

dotnet run


Tarayıcıdan erişim:
http://localhost:5000
Docker mapping ile http://localhost:32776

Docker ile Çalıştırma

Projede docker-compose.yml dosyası bulunur.

1. Docker Build & Up
docker-compose build
docker-compose up


SQL Server container adı: sqlserver

Web uygulaması: http://localhost:32776

2. Database Migrations

Host üzerinden:

dotnet ef database update


Container içinde:

docker exec -it ConfigurationManagement dotnet ef database update

Web Arayüzü

CRUD işlemleri: Index, Create, Edit, Delete

Filtreleme: Konfigürasyon adı ile client-side filtreleme
