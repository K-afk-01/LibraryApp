# LibraryMS - Kurulum Kılavuzu

## Gereksinimler
- .NET 8 SDK
- SQL Server LocalDB (Visual Studio ile gelir)
- Visual Studio 2022 veya VS Code

## Adımlar

### 1. Projeyi Açın
```
Visual Studio > Open > LibraryMS.csproj
```

### 2. Veritabanı Oluşturun
Package Manager Console (Tools > NuGet > Package Manager Console):
```
Add-Migration InitialCreate
Update-Database
```

### 3. Stored Procedure Ekleyin
SQL Server Object Explorer veya SSMS'te:
- `DatabaseSetup.sql` dosyasını açın
- `GetBookReport` stored procedure bölümünü çalıştırın

### 4. Uygulamayı Başlatın
```
F5 veya Ctrl+F5
```

### 5. Giriş Yapın
- **URL:** https://localhost:7001
- **Kullanıcı:** `admin`
- **Şifre:** `Admin123!`

## Proje Yapısı
| Controller | Model 1 | Model 2 |
|---|---|---|
| BookController | Book (Kitap) | Category (Kategori) |
| AuthorController | Author (Yazar) | Order (Sipariş) |
| AccountController | User Login | - |
| HomeController | Dashboard | Book Report (Stored Proc) |

## Özellikler
- ✅ Bootstrap 5 + Bootstrap Icons profesyonel arayüz
- ✅ TempData ile başarı/hata mesajları
- ✅ 2 controller (Book, Author) her biri 2 model CRUD
- ✅ Kullanıcı girişi (Session tabanlı)
- ✅ Stored Procedure (GetBookReport)
- ✅ Dropdown list (Yazar seçimi kitap formunda)
- ✅ Bootstrap Icons (tüm sayfalarda)
