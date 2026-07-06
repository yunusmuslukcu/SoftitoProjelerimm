using Data;
using Microsoft.EntityFrameworkCore;
using Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<User, Microsoft.AspNetCore.Identity.IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>();



var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted(); // Temporarily delete to apply new Identity schema
        context.Database.EnsureCreated(); // Ensure DB exists
        
        // Seed Categories
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Yazılım Hatası", Description = "Yazılım hataları, buglar ve sistem aksaklıkları" },
                new Category { Name = "Donanım Arızası", Description = "Bilgisayar, yazıcı, monitör ve diğer fiziksel donanım sorunları" },
                new Category { Name = "Ağ / Network", Description = "İnternet bağlantısı, kablolama, switch ve modem arızaları" },
                new Category { Name = "Sistem Erişim / Şifre", Description = "Kullanıcı hesapları, şifre sıfırlama ve erişim yetkileri" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        // Seed Users
        if (!context.Users.Any())
        {
            var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<User>>();
            var users = new List<User>
            {
                new User { FullName = "Ahmet Yılmaz", Email = "ahmet@btproject.com", UserName = "ahmet", Department = "Bilgi İşlem" },
                new User { FullName = "Ayşe Demir", Email = "ayse@btproject.com", UserName = "ayse", Department = "İnsan Kaynakları" },
                new User { FullName = "Mehmet Kaya", Email = "mehmet@btproject.com", UserName = "mehmet", Department = "Finans" },
                new User { FullName = "Fatma Çelik", Email = "fatma@btproject.com", UserName = "fatma", Department = "Pazarlama" }
            };
            
            foreach (var user in users)
            {
                var result = userManager.CreateAsync(user, "Password123!").GetAwaiter().GetResult();
            }
        }

        // Seed Tickets
        if (!context.Tickets.Any())
        {
            var categorySoftware = context.Categories.First(c => c.Name == "Yazılım Hatası");
            var categoryHardware = context.Categories.First(c => c.Name == "Donanım Arızası");
            var categoryNetwork = context.Categories.First(c => c.Name == "Ağ / Network");
            var categoryAccess = context.Categories.First(c => c.Name == "Sistem Erişim / Şifre");

            var userAhmet = context.Users.First(u => u.FullName == "Ahmet Yılmaz");
            var userAyse = context.Users.First(u => u.FullName == "Ayşe Demir");
            var userMehmet = context.Users.First(u => u.FullName == "Mehmet Kaya");
            var userFatma = context.Users.First(u => u.FullName == "Fatma Çelik");

            var tickets = new List<Ticket>
            {
                new Ticket { Title = "Bilgisayarım açılmıyor", Description = "Güç tuşuna basınca ışıklar yanmıyor, tamamen tepkisiz.", Status = TicketStatus.InProgress, CategoryId = categoryHardware.Id, UserId = userMehmet.Id },
                new Ticket { Title = "Outlook şifre sıfırlama", Description = "E-posta şifremi unuttum, giriş yapamıyorum.", Status = TicketStatus.Resolved, CategoryId = categoryAccess.Id, UserId = userAyse.Id },
                new Ticket { Title = "ERP uygulamasında kaydetme hatası", Description = "Fatura kaydederken SQL timeout hatası alıyorum.", Status = TicketStatus.Open, CategoryId = categorySoftware.Id, UserId = userFatma.Id },
                new Ticket { Title = "İnternet bağlantısı çok yavaş", Description = "Web sayfaları çok geç açılıyor, ağda kopmalar yaşanıyor.", Status = TicketStatus.Open, CategoryId = categoryNetwork.Id, UserId = userMehmet.Id },
                new Ticket { Title = "Yeni yazıcı kurulumu", Description = "Departmana yeni gelen yazıcının bilgisayarlara tanımlanması gerek.", Status = TicketStatus.Open, CategoryId = categoryHardware.Id, UserId = userAyse.Id },
                new Ticket { Title = "VPN bağlantı sorunu", Description = "Evden çalışırken VPN ile şirkete bağlanamıyorum.", Status = TicketStatus.InProgress, CategoryId = categoryNetwork.Id, UserId = userFatma.Id }
            };
            context.Tickets.AddRange(tickets);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // Log errors or handle
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Portal}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

