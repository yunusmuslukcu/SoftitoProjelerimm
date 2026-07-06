var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// .NET 10 dahili OpenAPI servisi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // /openapi/v1.json adresinde API dökümanı sunar
    app.MapOpenApi();
}

// Swagger UI - CDN üzerinden yüklenir, ek paket gerektirmez
app.MapGet("/swagger", () => Results.Content("""
<!DOCTYPE html>
<html lang="tr">
<head>
    <meta charset="utf-8" />
    <title>VeloRent API - Swagger UI</title>
    <link rel="stylesheet" href="https://unpkg.com/swagger-ui-dist@5.17.14/swagger-ui.css" />
    <style>
        body { margin: 0; background: #fafafa; }
        .topbar { display: none; }
    </style>
</head>
<body>
    <div id="swagger-ui"></div>
    <script src="https://unpkg.com/swagger-ui-dist@5.17.14/swagger-ui-bundle.js"></script>
    <script>
        SwaggerUIBundle({
            url: '/openapi/v1.json',
            dom_id: '#swagger-ui',
            deepLinking: true,
            presets: [SwaggerUIBundle.presets.apis, SwaggerUIBundle.SwaggerUIStandalonePreset],
            layout: 'BaseLayout'
        });
    </script>
</body>
</html>
""", "text/html"));

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
