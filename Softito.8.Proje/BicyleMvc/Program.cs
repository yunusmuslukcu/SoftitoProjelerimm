using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// HTTP Client ekleme
builder.Services.AddHttpClient("BicyleApi", client =>
{
    // API adresi - http profili ile aynı port
    client.BaseAddress = new Uri("http://localhost:5162/");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
});

// ApiService kaydı
builder.Services.AddScoped<BicyleMvc.Services.ApiService>();

// Cookie tabanlı kimlik doğrulama ekleme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulamayı etkinleştir
app.UseAuthorization();  // Yetkilendirmeyi etkinleştir

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Site}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

