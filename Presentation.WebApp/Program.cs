using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Presentation.WebApp.Routing;
using Presentation.WebApp.Services.MenuNavigation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new MenuUrlRewriter()));
});

builder.Services.AddApplication(builder.Configuration, builder.Environment);
builder.Services.AddInfrastrcuture(builder.Configuration, builder.Environment);
builder.Services.AddScoped<IMenuNavigationService, MenuNavigationService>();

builder.Services.AddSession();
builder.Services.AddRouting(x => x.LowercaseUrls = true);

var app = builder.Build();

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
