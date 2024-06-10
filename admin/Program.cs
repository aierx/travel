using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using webapi.authentication;

namespace webapi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(configurePolicy =>
            {
                configurePolicy.WithOrigins("http://localhost:3000", "http://192.168.1.7:3000","http://127.0.0.1:3000").AllowAnyHeader()
                    .AllowAnyMethod().AllowCredentials();
            });
        });
        builder.Services.AddAuthentication(
            option =>
            {
                option.DefaultScheme = "auth";
                option.AddScheme<MyAuthenticationHandler>("auth", "auth");
            });
        builder.Services.AddRazorPages();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.Cookie.MaxAge = TimeSpan.FromMinutes(24 * 60);
            options.Cookie.HttpOnly = false;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ForwardForbid = "auth";
            options.ForwardChallenge = "auth";
        });
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql("Server=localhost;Database=admin;Uid=root;Pwd=123456;",
                new MySqlServerVersion("5.7.44"));
        });

        var app = builder.Build();
        app.UseCors();
        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute("aaa", "/app/{*a}", new { controller = "App", action = "Index" });
        app.MapControllers();
        app.Run();
    }
}