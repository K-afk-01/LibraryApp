-- ============================================================
-- LibraryMS - Veritabani Kurulum Scripti
-- CSE206 Web Technologies and Programming - Final Assignment
-- Grup: Abdulkadir Kamil (241805071), Ertugrul Yildirim (241805070), Arda Savran (241805030)
-- ============================================================

-- 1. Veritabani olustur
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LibraryMSDb')
BEGIN
    CREATE DATABASE LibraryMSDb;
END
GO

USE LibraryMSDb;
GO

-- 2. Tablolar EF Core Migration ile otomatik olusturulur.
--    Asagidaki komutu terminal'de calistirin:
--    dotnet ef migrations add InitialCreate
--    dotnet ef database update

-- ============================================================
-- STORED PROCEDURE: GetBookReport
-- Tum kitaplari yazar ve kategori bilgileriyle dondurur.
-- HomeController.cs > Report() action'inda cagirilir.
-- ============================================================
IF OBJECT_ID('dbo.GetBookReport', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetBookReport;
GO

CREATE PROCEDURE dbo.GetBookReport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        b.BookId,
        b.Title,
        b.ISBN,
        b.Price,
        a.FirstName + ' ' + a.LastName  AS AuthorName,
        c.Name                           AS CategoryName,
        b.Stock
    FROM
        Books b
        INNER JOIN Authors    a ON b.AuthorId    = a.AuthorId
        INNER JOIN Categories c ON b.CategoryId  = c.CategoryId
    ORDER BY
        b.Title;
END
GO

-- Test: EXEC dbo.GetBookReport;

-- ============================================================
-- KULLANIM TALIMATLARI
-- ============================================================
-- 1. Visual Studio / terminal'de projeyi acin
-- 2. appsettings.json dosyasindaki baglanti dizesini kontrol edin
-- 3. Package Manager Console'da calistirin:
--      Add-Migration InitialCreate
--      Update-Database
-- 4. Bu SQL scriptini SQL Server Management Studio veya
--    Azure Data Studio'da calistirin (Stored Procedure icin)
-- 5. Uygulamayi calistirin: dotnet run
-- 6. Tarayicida https://localhost:7001 adresine gidin
-- 7. Giris: admin / Admin123!
