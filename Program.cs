using Agenda.Context;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração prioritária: ambiente > appsettings
builder.Configuration
    .AddEnvironmentVariables() // Para Railway/Variáveis de ambiente
    .AddJsonFile("appsettings.json", optional: true); // Para desenvolvimento

// Configuração do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // 1. Tenta obter do Railway (formato postgres://)
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    // 2. Se não encontrou, usa appsettings.json
    if (string.IsNullOrEmpty(databaseUrl))
    {
        databaseUrl = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    // 3. Se for formato Railway, converte
    if (!string.IsNullOrEmpty(databaseUrl) && databaseUrl.StartsWith("postgres://"))
    {
        databaseUrl = ConvertRailwayDbUrl(databaseUrl);
    }

    // Configura o PostgreSQL
    options.UseNpgsql(databaseUrl);
});

// Método para converter URL do Railway
static string ConvertRailwayDbUrl(string railwayUrl)
{
    var uri = new Uri(railwayUrl);
    var userInfo = uri.UserInfo.Split(':');

    return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};" +
           $"Username={userInfo[0]};Password={userInfo[1]};" +
           "SSL Mode=Require;Trust Server Certificate=true";
}

// Configuração do MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do Ambiente
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configuração importante para o Railway
app.UseForwardedHeaders();
app.Use((context, next) =>
{
    if (context.Request.Headers["X-Forwarded-Proto"] == "https")
    {
        context.Request.Scheme = "https";
    }
    return next();
});

// Configurações padrão
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Rota principal definida para sua Agenda
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ModeloAgendas}/{action=Index}/{id?}");

app.Run();