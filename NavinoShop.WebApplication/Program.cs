using Blogs.Query.Bootstrapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NavinoShop.WebApplication.Utility;
using Users.Query.Bootstrapper;


var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
var Configuration = builder.Configuration;
var ConnectionString = Configuration.GetConnectionString("DefultConnection");
Services.AddControllersWithViews();
Services.AddRazorPages();

#region Bootstrappers

DependencyBootstrapper.Congig(Services);
BlogQueryBootstrapper.Config(Services, ConnectionString);
UserQueryBootstrapper.Config(Services, ConnectionString);
#endregion
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapRazorPages();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
