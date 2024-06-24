
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;

namespace SignalRChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
   
           // Add services to the container.
            builder.Services.AddRazorPages();
            //  builder.Services.AddSignalR();
            //  builder.Services.AddSingleton<IRepository, Repository>();
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));
            var app = builder.Build();
            app.UseForwardedHeaders();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
               
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            
            //app.UseAuthorization();
            //app.UseAuthentication();

            app.MapRazorPages();
     
            app.Run();
        }
    }
}