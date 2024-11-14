using KA.Application.Interfaces;
using KA.Application.Services;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using KA.Infra.Data.Repositories;
using KA.Infra.Data.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KA.Ioc
{
    public static class SCExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Add connection string for DB using MYSql
            services.AddDbContext<KADbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 2)),
                    b => b.MigrationsAssembly(typeof(KADbContext).Assembly.FullName));
            });

            //JWT settings
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.GetSection("issuer").Value,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.GetSection("audience").Value,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("SecretKey").Value)) // Secret Key used to sign the token
                    };
                });


            services.AddAuthorization();

            //Repositories

            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReceiptRepository, ReceiptRepository>();

            //Services
            services.AddScoped<IAuthenticatedService, AuthenticatedService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IReceiveService, ReceiveService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReceiveService, ReceiveService>();
            services.AddScoped<IPromotionService, PromotionService>();

            return services;
        }


        public static IApplicationBuilder AddConfigurationInfrastructure(this IApplicationBuilder app, IServiceProvider services)
        {
            //Populate DB
            using (var scope = services.CreateScope())
            {
                var servicesPro = scope.ServiceProvider;
                SeedData.Initialize(servicesPro);
            }
            return app;
        }
    }
}
