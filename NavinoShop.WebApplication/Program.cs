using Microsoft.EntityFrameworkCore;
using NavinoShop.WebApplication.Utility;
using Seos.Query.Bootstrapper;
using Site.Query.Bootstrapper;
using Users.Query.Bootstrapper;
using Blogs.Query.Bootstrapper;
using Comments.Query.Bootstrapper;
using Emails.Query.Bootstrapper;
using PostModule.Query.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
var Configuration = builder.Configuration;
var ConnectionString = Configuration.GetConnectionString("DefultConnection");
Services.AddControllersWithViews();
Services.AddRazorPages();

#region Bootstrappers

DependencyBootstrapper.Congig(Services,ConnectionString);
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
    x.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    x.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

});




app.Run();
