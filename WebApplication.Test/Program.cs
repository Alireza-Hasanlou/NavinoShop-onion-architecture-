
using Refit;
using WebApplication.Test.Interfaces;

var builder =Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddControllersWithViews();
// Register Refit Client
builder.Services.AddRefitClient<IPostApiService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://localhost:44325");
        c.Timeout = TimeSpan.FromSeconds(30);
    });
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
