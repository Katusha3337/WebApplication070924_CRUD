using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервиса в контейнер
builder.Services.AddSingleton<DatabaseService>(sp => new DatabaseService("Server=DESKTOP-M2FVLS1\\SQLEXPRESS;Database=testdb;Trusted_Connection=True;TrustServerCertificate=true;"));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Маршрут для создания записи
app.MapPost("/create", (string tableName, Dictionary<string, object> data, DatabaseService dbService) =>
{
    dbService.Create(tableName, data);
    return Results.Ok("Record created successfully");
});

// Маршрут для чтения записей
app.MapGet("/read", (string tableName, string condition, DatabaseService dbService) =>
{
    var results = dbService.Read(tableName, condition);
    return Results.Ok(results);
});

// Маршрут для обновления записи
app.MapPut("/update", (string tableName, Dictionary<string, object> data, string condition, DatabaseService dbService) =>
{
    dbService.Update(tableName, data, condition);
    return Results.Ok("Record updated successfully");
});

// Маршрут для удаления записи
app.MapDelete("/delete", (string tableName, string condition, DatabaseService dbService) =>
{
    dbService.Delete(tableName, condition);
    return Results.Ok("Record deleted successfully");
});

app.Run();