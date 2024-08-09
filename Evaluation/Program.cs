using Microsoft.EntityFrameworkCore;
using Evaluation.Models.UnitTest;
using Evaluation.Models.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Evaluation.Models.Cnx;

var builder = WebApplication.CreateBuilder(args);

// ------- UNIT TEST --------
//MainTest.Main(args);
//Validation.Main(args);
// --------------------------

// --- Ajout des Services Singleton au conteneur ---
builder.Services.AddSingleton<CustomLogger>();
builder.Services.AddSingleton<ConsoleInterceptor>();
// -------------------------------------------------

// -------------------------------------------------

// Add services to the container.
builder.Services.AddControllersWithViews();

// ------- Pour DBCONTEXT --------
builder.Services.AddDbContext<PsqlContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("NpgsqlConnectionString"));
});
// -------------------------------

// ------- Authentications -------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.AccessDeniedPath = "/Admin/AccessDenied";
        option.LoginPath = "/Equipe/EquipeSignIn";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
// -------------------------------

// ------ Pour les SESSIONS ------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// -------------------------------

var app = builder.Build();

// ------ Intentionally Warm-up Context ------
ChargeContext.Main(app.Services);
// -------------------------------------------

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

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Equipe}/{action=EquipeHome}/{id?}");

app.Run();
