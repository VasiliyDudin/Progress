using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Entity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("CORE_CONNECTION_STRING"));
    option.EnableDetailedErrors();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
