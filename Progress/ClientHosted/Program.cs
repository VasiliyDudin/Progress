using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDirectoryBrowser();


var app = builder.Build();

app.UseDefaultFiles();
app.UseFileServer(enableDirectoryBrowsing: true);

app.Run();
