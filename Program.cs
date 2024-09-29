using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ���������� ������� � ���������
builder.Services.AddSingleton<DatabaseService>(sp => new DatabaseService("Server=DESKTOP-M2FVLS1\\SQLEXPRESS;Database=testdb;Trusted_Connection=True;TrustServerCertificate=true;"));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// ������� ��� �������� ������
app.MapPost("/create", (string tableName, Dictionary<string, object> data, DatabaseService dbService) =>
{
    dbService.Create(tableName, data);
    return Results.Ok("Record created successfully");
});

// ������� ��� ������ �������
app.MapGet("/read", (string tableName, string condition, DatabaseService dbService) =>
{
    var results = dbService.Read(tableName, condition);
    return Results.Ok(results);
});

// ������� ��� ���������� ������
app.MapPut("/update", (string tableName, Dictionary<string, object> data, string condition, DatabaseService dbService) =>
{
    dbService.Update(tableName, data, condition);
    return Results.Ok("Record updated successfully");
});

// ������� ��� �������� ������
app.MapDelete("/delete", (string tableName, string condition, DatabaseService dbService) =>
{
    dbService.Delete(tableName, condition);
    return Results.Ok("Record deleted successfully");
});

app.Run();