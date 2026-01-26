using Microsoft.EntityFrameworkCore;
using NavinoShop.WebApplication.Utility;
using Seos.Query.Bootstrapper;
using Site.Query.Bootstrapper;
using Users.Query.Bootstrapper;
using Blogs.Query.Bootstrapper;
using Comments.Query.Bootstrapper;
using Emails.Query.Bootstrapper;
using PostModule.Query.Bootstrapper;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
var Configuration = builder.Configuration;
var ConnectionString = Configuration.GetConnectionString("DefultConnection");
Services.AddControllersWithViews();
Services.AddRazorPages();
Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
#region Bootstrappers

DependencyBootstrapper.Congig(Services, ConnectionString);
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
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(x =>
{

    x.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}/{Slug?}");
    x.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    x.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Blog}/{action=Index}/{id?}");
});
app.MapAreaControllerRoute("areas", "UserPanel", "UserPanel/{controller=Panel}/{action=Profile}/{id?}");
app.Run();
