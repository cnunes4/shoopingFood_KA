using KA.Infra.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace KA.Ioc
{
    public static class Swagger_SCExtensions
    {
        public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                { 
                    Name = "Authorization",
                    Type= SecuritySchemeType.ApiKey, 
                    BearerFormat ="JWT", 
                    In= ParameterLocation.Header, 
                    Description ="JSON WEB TOKEN "
                });


                c.AddSecurityRequirement( new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference= new OpenApiReference()
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            }
                        },
                         new string [] {}
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "KA API",
                    Version = "v1"
                });
            });

            return services;
        }


        public static IApplicationBuilder AddConfigurationSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
            }
            return app;
        }

    }
}
