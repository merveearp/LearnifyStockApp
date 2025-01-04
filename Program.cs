using System.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Extensions;
using LearnifyStockApp.Models.Repositories;
using Microsoft.Data.SqlClient;
using LearnifyStockApp.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDbConnection>(o => new SqlConnection(builder.Configuration.GetConnectionString("DataConnection")));

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<SupplierRepository>();
builder.Services.AddScoped<SaleRepository>();

builder.Services.AddNotyf(options =>
{
    options.DurationInSeconds=3;
    options.IsDismissable=true;
    options.Position=NotyfPosition.TopRight;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
