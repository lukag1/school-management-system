using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using projekatPPP.Data;
using projekatPPP.Services;
using projekatPPP.Repositories;
using Microsoft.AspNetCore.Identity;
using projekatPPP.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ako prelaziš na Identity, promeni ApplicationUser da nasledjuje IdentityUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// registruj repozitorijume i servise
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PredmetRepository>();
builder.Services.AddScoped<PredmetService>();
builder.Services.AddScoped<OcenaRepository>();
builder.Services.AddScoped<OcenaService>();
builder.Services.AddScoped<OdeljenjeRepository>(); // Dodaj ovu liniju
builder.Services.AddScoped<OdeljenjeService>();    // Dodaj ovu liniju

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

// Seed roles i users pri startup-u
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedUsersAndRolesAsync(services);
}

app.Run();

static async Task SeedUsersAndRolesAsync(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Roles
    var roles = new[] { "Administrator", "Nastavnik", "Ucenik" };
    foreach (var r in roles)
    {
        if (!await roleManager.RoleExistsAsync(r))
            await roleManager.CreateAsync(new IdentityRole(r));
    }

    // Admin user
    var adminEmail = "admin@local.test";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Ime = "Admin",
            Prezime = "Admin",
            IsApproved = true
        };
        var res = await userManager.CreateAsync(admin, "Admin123!"); // razvojna lozinka
        if (res.Succeeded) await userManager.AddToRoleAsync(admin, "Administrator");
    }

    // Nastavnik user
    var nastEmail = "nastavnik@local.test";
    var nastavnik = await userManager.FindByEmailAsync(nastEmail);
    if (nastavnik == null)
    {
        nastavnik = new ApplicationUser
        {
            UserName = nastEmail,
            Email = nastEmail,
            Ime = "Marko",
            Prezime = "Nastavnik",
            IsApproved = true
        };
        var res2 = await userManager.CreateAsync(nastavnik, "Nastavnik123!");
        if (res2.Succeeded) await userManager.AddToRoleAsync(nastavnik, "Nastavnik");
    }

    // Ucenik user (neodobren — admin treba da odobri)
    var ucenikEmail = "ucenik@local.test";
    var ucenik = await userManager.FindByEmailAsync(ucenikEmail);
    if (ucenik == null)
    {
        ucenik = new ApplicationUser
        {
            UserName = ucenikEmail,
            Email = ucenikEmail,
            Ime = "Jovana",
            Prezime = "Ucenik",
            IsApproved = true // admin odobrenje potrebno
        };
        var res3 = await userManager.CreateAsync(ucenik, "Ucenik123!");
        if (res3.Succeeded) await userManager.AddToRoleAsync(ucenik, "Ucenik");
    }
}
