using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Web_CSV_Json_XML_reader.Data.DB;
using Web_CSV_Json_XML_reader.Data.DB.Entities;
using Web_CSV_Json_XML_reader.Data.Managers;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EFContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("EFContext")));

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = 256000;
}); //необходимо дл€ передачи бќльшего количества значений формы, потом уменьшить до вмен€емого размера

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<IXMLManager, XMLHandlerOld>();
builder.Services.AddScoped<IJSONManager, JSONHandlerOld>();
builder.Services.AddScoped<ICSVManager, CSVHandler>();
builder.Services.AddScoped<IFileSaveManager, FileSaveManager>();
builder.Services.AddScoped<IViewModelsCreator, ViewModelsCreator>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/Index");
builder.Services.AddAuthorization();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



// ѕеределать способ сохранени€ изменЄнного файла (без необходимости передавать и хранить на странице исходный код файла)
// 
//