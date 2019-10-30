using Joonasw.AadTestingDemo.API.Authorization;
using Joonasw.AadTestingDemo.API.OpenApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using AuthenticationOptions = Joonasw.AadTestingDemo.API.Options.AuthenticationOptions;

namespace Joonasw.AadTestingDemo.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Authentication
            AuthenticationOptions authenticationOptions = _configuration.GetSection("Authentication").Get<AuthenticationOptions>();
            services.Configure<AuthenticationOptions>(_configuration.GetSection("Authentication"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.Authority = authenticationOptions.Authority;
                    o.Audience = authenticationOptions.ClientId;
                });
            services.AddSingleton<IClaimsTransformation, ScopeClaimSplitTransformation>();

            // Authorization
            services.AddAuthorization(o =>
            {
                // Require callers to have at least one valid permission by default
                o.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new AnyValidPermissionRequirement())
                    .Build();
                // Create a policy for each action that can be done in the API
                foreach (string action in Actions.All)
                {
                    o.AddPolicy(action, policy => policy.AddRequirements(new ActionAuthorizationRequirement(action)));
                }
            });
            services.AddSingleton<IAuthorizationHandler, AnyValidPermissionRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, ActionAuthorizationRequirementHandler>();

            // Swagger / OpenAPI document setup
            services.AddSwaggerGen(o =>
            {
                // Setup our document's basic info
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Things API",
                    Version = "1.0"
                });

                // Define that the API requires OAuth 2 tokens
                o.AddSecurityDefinition("aad-jwt", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        // We only define implicit though the UI does support authorization code, client credentials and password grants
                        // We don't use authorization code here because it requires a client secret, which makes this sample more complicated by introducing secret management
                        // Client credentials could work, but not when the UI client id == API client id. We'd need a separate registration and granting app permissions to that. And also needs a secret.
                        // Password grant we don't use because... you shouldn't be using it.
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authenticationOptions.AuthorizationUrl),
                            Scopes = DelegatedPermissions.All.ToDictionary(p => $"{authenticationOptions.ApplicationIdUri}/{p}")
                        }
                    }
                });

                // Add security requirements to operations based on [Authorize] attributes
                o.OperationFilter<OAuthSecurityRequirementOperationFilter>();

                // Include XML comments to documentation
                string xmlDocFilePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Joonasw.AadTestingDemo.API.xml");
                o.IncludeXmlComments(xmlDocFilePath);
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IOptions<AuthenticationOptions> authenticationOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });

            // Swagger / OpenAPI document
            app.UseSwagger();
            // The interactive documentation
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                o.OAuthClientId(authenticationOptions.Value.ClientId);
            });
        }
    }
}
