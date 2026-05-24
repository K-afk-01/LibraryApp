# Migrations

EF Core migration'larini otomatik olusturmak icin:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Sonra DatabaseSetup.sql'deki stored procedure'u SQL Server'da calistirin.
