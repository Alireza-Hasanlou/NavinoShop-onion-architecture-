using Blogs.Query.Bootstrapper;
using Comments.Query.Bootstrapper;
using Emails.Query.Bootstrapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using NavinoShop.WebApplication.Utility;
using NavinoShop.WebApplication.Utility.Filters;
using PostModule.Query.Bootstrapper;
using Seos.Query.Bootstrapper;
using Site.Query.Bootstrapper;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Users.Query.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
var Configuration = builder.Configuration;
var ConnectionString = Configuration.GetConnectionString("DefultConnection");

Services.AddControllersWithViews();
Services.AddRazorPages();
Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
Services.Configure<SiteData>(Configuration.GetSection("SiteData"));

#region Bootstrappers
DependencyBootstrapper.Congig(Services, ConnectionString);
#endregion

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<NotFoundFilter>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseStatusCodePagesWithReExecute("/Error/StatusCode404");

app.UseRouting();
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{Slug?}");

app.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Blog}/{action=Index}/{id?}");

app.MapAreaControllerRoute("areas", "UserPanel", "UserPanel/{controller=Panel}/{action=Profile}/{id?}");


app.MapControllerRoute(
    name: "notFound",
    pattern: "{*url}",
    defaults: new { controller = "Error", action = "NotFound" });

app.Run();