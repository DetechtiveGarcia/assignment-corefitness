using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastrcuture(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {

        services.AddDbContext<PersistenceContext>(x => x.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        services.AddIdentity<AppUser, IdentityRole>(x =>
        {

        }).AddEntityFrameworkStores<PersistenceContext>().AddDefaultTokenProviders();
        return services;
    }
}
