using IoTSystemApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı (DbContext Kaydı)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// 2. Identity Altyapısının Kurulması
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();

// 3. Controller Servislerinin Eklenmesi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Geliştirme Ortamı İçin Swagger Aktif Edilmesi
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// 5. Kimlik Doğrulama ve Yetkilendirme Middleware'leri
app.UseAuthentication();
app.UseAuthorization();

// 6. Identity Giriş/Kayıt (/login, /register) Endpoint'lerinin Haritalanması
app.MapIdentityApi<IdentityUser>();

// Controller Route'larının Haritalanması
app.MapControllers();

app.Run();