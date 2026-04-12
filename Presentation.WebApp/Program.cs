using Application.Extensions;
using Infrastructure.Extensions.Identity;
using Infrastructure.Extensions.Persistence;
using Infrastructure.Identity;
using Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Presentation.WebApp.Routing;
using Presentation.WebApp.Services.MenuNavigation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new MenuUrlRewriter()));
});

builder.Services.AddApplication(builder.Configuration, builder.Environment);
builder.Services.AddPersistence(builder.Configuration, builder.Environment);
builder.Services.AddIdentityService();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/authentication/sign-in";
        options.AccessDeniedPath = "/error/accessdenied";
        options.Cookie.Name = "CoreFitness.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IMenuNavigationService, MenuNavigationService>();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "CoreFitness.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddRouting(x => x.LowercaseUrls = true);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    await DataSeeder.SeedRoleAsync(roleManager);
    await DataSeeder.SeedAdminAsync(userManager);
}

app.UseExceptionHandler("/error/500");
app.UseStatusCodePagesWithReExecute("/error/{0}");


app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
