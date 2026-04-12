using Application.Abstractions.Services;
using Application.Services.FitnessClasses;
using Application.Services.Members;
using Application.Services.Memberships;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {

        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IFitnessClassService, FitnessClassService>();
        return services;
    }
}
