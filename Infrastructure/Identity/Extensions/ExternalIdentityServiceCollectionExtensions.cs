using AspNet.Security.OAuth.GitHub;
using Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;


namespace Infrastructure.Identity.Extensions;

public static class ExternalIdentityServiceCollectionExtensions
{
    public static IServiceCollection AddExternalIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationBuilder = services.AddAuthentication();

        var gitHubOptions = configuration
            .GetSection(GitHubAuthOptions.SectionName)
            .Get<GitHubAuthOptions>();

        if (gitHubOptions is not null && !string.IsNullOrWhiteSpace(gitHubOptions.ClientId) && !string.IsNullOrWhiteSpace(gitHubOptions.ClientSecret))
        {
            authenticationBuilder.AddGitHub(GitHubAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = gitHubOptions.ClientId;
                options.ClientSecret = gitHubOptions.ClientSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-github";

                options.Scope.Add("user:email");

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");

                options.ClaimActions.MapJsonKey("urn:github:login", "login");
                options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
                options.ClaimActions.MapJsonKey("urn:github:url", "html_url");

                options.SaveTokens = true;
            });



            var googleOptions = configuration
                .GetSection(GoogleAuthOptions.SectionName)
                .Get<GoogleAuthOptions>();

            if (googleOptions is not null && !string.IsNullOrWhiteSpace(googleOptions.ClientId) && !string.IsNullOrWhiteSpace(googleOptions.ClientSecret))
            {
                authenticationBuilder.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    options.ClientId = googleOptions.ClientId;
                    options.ClientSecret = googleOptions.ClientSecret;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.CallbackPath = "/signin-google";

                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

                    options.SaveTokens = true;
                });
            }

            //var microsoftOptions = configuration
            //    .GetSection(MicrosoftAuthOptions.SectionName)
            //    .Get<MicrosoftAuthOptions>();

            //if (microsoftOptions is not null && !string.IsNullOrWhiteSpace(microsoftOptions.ClientId) && !string.IsNullOrWhiteSpace(microsoftOptions.ClientSecret))
            //{
            //    authenticationBuilder.AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options =>
            //    {
            //        options.ClientId = microsoftOptions.ClientId;
            //        options.ClientSecret = microsoftOptions.ClientSecret;
            //        options.SignInScheme = IdentityConstants.ExternalScheme;
            //        options.CallbackPath = "/signin-microsoft";

            //        options.SaveTokens = true;
            //    });


                
            //}
        }

        return services;
    }
}