using KA.Ioc;

namespace KA.Api1
{
    public class Startup
    {

        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddInfrastructureSwagger(_configuration);
            services.AddInfrastructure(_configuration);

            services.AddAuthorization();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            app.AddConfigurationSwagger(env);
            app.AddConfigurationInfrastructure(services);
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
