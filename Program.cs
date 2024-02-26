using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IFileManager, FileManager>();
builder.Services.AddTransient<IXMLManager, XMLHandlerOld>();
builder.Services.AddTransient<IJSONManager, JSONHandlerOld>();
builder.Services.AddTransient<ICSVManager, CSVHandler>();
builder.Services.AddTransient<IFileSaveManager, FileSaveManager>();
builder.Services.AddTransient<IViewModelsCreator, ViewModelsCreator>();

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
    //pattern: "{controller=Account}/{action=Index3}/{id?}");


app.Run();

