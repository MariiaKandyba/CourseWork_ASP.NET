using Client.Areas.Services;
using Client.Models;
using Client.Services.Auth;
using Client.Services.Order;
using Client.Services.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Add services to the container.
// Add services to the container.
builder.Services.AddHttpClient<AuthService>();
builder.Services.AddHttpClient<ProductService>();
builder.Services.AddHttpClient<OrderService>();
builder.Services.AddHttpClient<UserService>();
builder.Services.AddHttpClient<StockProductService>();



builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});


builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login"; 
        options.LogoutPath = "/Auth/Logout"; 
        options.ExpireTimeSpan = TimeSpan.FromHours(1); 
    });
builder.Services.AddHttpContextAccessor(); 



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
