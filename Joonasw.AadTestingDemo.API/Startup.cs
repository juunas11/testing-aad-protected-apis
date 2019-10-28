using Joonasw.AadTestingDemo.API.Authorization;
using Joonasw.AadTestingDemo.API.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

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
            var authenticationOptions = _configuration.GetSection("Authentication").Get<AuthenticationOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.Authority = authenticationOptions.Authority;
                    o.Audience = authenticationOptions.ClientId;
                });
            services.AddSwaggerGen(o =>
            {
                // Setup our document's basic info
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Things API",
                    Version = "1.0"
                });

                o.AddSecurityDefinition("aad-jwt", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authenticationOptions.AuthorizationUrl),
                            Scopes = new Dictionary<string, string>
                            {
                                [authenticationOptions.ApplicationIdUri + "/" + DelegatedPermissions.ReadThings] = "Read things"
                            }
                        }
                    }
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "aad-jwt"
                            },
                            UnresolvedReference = true
                        },
                        new List<string>
                        {
                            authenticationOptions.ApplicationIdUri + "/" + DelegatedPermissions.ReadThings
                        }
                    }
                });

                // Include XML comments to documentation
                string xmlDocFilePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Joonasw.AadTestingDemo.API.xml");
                o.IncludeXmlComments(xmlDocFilePath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                o.OAuthClientId(_configuration["Authentication:ClientId"]);
            });
        }
    }
}
