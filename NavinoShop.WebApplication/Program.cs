using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NavinoShop.WebApplication.Utility;
using NavinoShop.WebApplication.Utility.AutoMapper;
using NavinoShop.WebApplication.Utility.Filters;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ZarinPal.Class;


var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
var Configuration = builder.Configuration;
var ConnectionString = Configuration.GetConnectionString("DefultConnection");
Services.AddControllersWithViews();
Services.AddRazorPages();
Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
Services.Configure<SiteData>(Configuration.GetSection("SiteData"));
builder.Services.AddScoped<Payment>();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("seller", typeof(SellerSlugConstraint));
});
#region Bootstrappers
DependencyBootstrapper.Congig(Services, ConnectionString);
#endregion

Services.AddControllersWithViews(options =>
{
    options.Filters.Add<NotFoundFilter>();
});
Services.AddAutoMapper(x =>
{
    x.AddMaps(typeof(MappingProfile));
});
Services.AddDistributedMemoryCache();

Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
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