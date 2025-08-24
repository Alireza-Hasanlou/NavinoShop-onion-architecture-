using Blogs.Query.Bootstrapper;
using NavinoShop.WebApplication.Utility;
using Users.Query.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
var Configuration = builder.Configuration;
var ConnectionString = Configuration.GetConnectionString("DefultConnection");
Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
