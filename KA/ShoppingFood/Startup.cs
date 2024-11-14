using ShoopingFood.Interfaces;
using ShoopingFood.Services;

namespace ShoppingFood
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddLogging();
            //COOKIE FO LOGIN
            services.AddAuthentication("KACookieAuthScheme")
            .AddCookie("KACookieAuthScheme");


            services.AddControllersWithViews();

            //SESSION
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //API'S
            services.AddHttpClient("FoodApi", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5019");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient("LoginApi", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5019");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            //Services
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IPromotionService, PromotionService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
