using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Newtonsoft.Json;
using AuthService.Utility;

namespace AuthService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
              .AddControllers(x => { x.Filters.Add(typeof(WebsiteExceptionFilter), 1); })
              .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddMemoryCache();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notifications.Api", Version = "v1" }); });

            services
              .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(x => {
                  x.Cookie.Domain = $".auth";
                  x.Cookie.Name = "auth";
                  x.ExpireTimeSpan = TimeSpan.FromMinutes(15);
              });

            services
              .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           
            // TODO: add database, and seed database
            services
              //.AddTransient<IDatabaseSeeder, DatabaseSeeder>()
            .AddSingleton(x => {
                  var assemblies = new List<Assembly> {
            //typeof(DatabaseSeeder).Assembly
                };

                  return assemblies;
              //})
              //.AddScoped<IRepository, Repository>();

            // TOOD: add website policies

              });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notifications.Api v1"));
            }

            app.UseRouting();

            app
              .UseAuthentication()
              .UseCookiePolicy(new CookiePolicyOptions())
              .UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
