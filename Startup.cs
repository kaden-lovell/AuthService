using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Newtonsoft.Json;

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
            //services
              //.Configure<Application>(x => _configuration.GetSection("Application").Bind(x))
              //.Configure<Authentication>(x => _configuration.GetSection("Authentication").Bind(x))
              //.Configure<ConnectionStrings>(x => _configuration.GetSection("ConnectionStrings").Bind(x))
              //.Configure<Environment>(x => _configuration.GetSection("Environment").Bind(x))
              //.Configure<Logging>(x => _configuration.GetSection("Logging").Bind(x))
              //.Configure<Network>(x => _configuration.GetSection("Network").Bind(x));

            //var authentication = new Authentication();
            //var environment = new Environment();
            //var network = new Network();

            //_configuration.GetSection("Authentication").Bind(authentication);
            //_configuration.GetSection("Environment").Bind(environment);
            //_configuration.GetSection("Network").Bind(network);

            //services
              //.AddDataProtection()
              //.PersistKeysToFileSystem(new DirectoryInfo(authentication.KeyDirectory))
              //.SetApplicationName(Constants.Authentication.APPLICATION_NAME);
            services
              .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(x => {
                  x.Cookie.Domain = $".auth";
                  x.Cookie.Name = "auth";
                  x.ExpireTimeSpan = TimeSpan.FromMinutes(15);
              });

            services
              //.AddSingleton<IEnvironmentAccessor>(x => new EnvironmentAccessor(x.GetService<IOptions<Environment>>(), x.GetService<IOptions<Network>>()))
              .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services
              //.AddTransient<IDatabaseSeeder, DatabaseSeeder>()
            .AddSingleton(x => {
                  var assemblies = new List<Assembly> {
            //typeof(DatabaseSeeder).Assembly
                };

                  return assemblies;
              //})
              //.AddSingleton<DatabaseInitializer>()
              //.AddSingleton<AccountsConfigService>()
              //.AddDbContext<ApiContext>()
              //.AddScoped(x => {
                  //var context = (IPersistenceContext)x.GetService<ApiContext>();
                  //var databaseInitializer = x.GetService<DatabaseInitializer>();
                  //var accountsConfigService = x.GetService<AccountsConfigService>();

                  //if (!databaseInitializer.IsInitialized(context))
                  //{
                      //lock (_lock)
                  //    {
                  //        if (!databaseInitializer.IsInitialized(context))
                  //        {
                  //            FileUtility.Retry(4, 2, () => {
                  //                using var scope = x.CreateScope();

                  //                var c = (IPersistenceContext)scope.ServiceProvider.GetService<ApiContext>();

                  //                c.IsInitializing = true;

                  //                databaseInitializer.Initialize(c);
                  //            });
                  //        }
                  //    }
                  //}

              //    if (!accountsConfigService.IsInitialized())
              //    {
              //        lock (_lock)
              //        {
              //            if (!accountsConfigService.IsInitialized())
              //            {
              //                FileUtility.Retry(4, 2, async () => { await accountsConfigService.Initialize(); });
              //            }
              //        }
              //    }

              //    context.BeginTransaction();

              //    return context;
              //})
              //.AddScoped<IRepository, Repository>();

            // keep services alphabetical
            //services
              //.AddScoped<ConnectService>()
              //.AddScoped<NotificationService>()
              //.AddScoped<NotificationSearchService>()
              //.AddScoped<EmailConfigService>()
              //.AddScoped<OrganizationService>()
              //.AddScoped<SystemService>()
              //.AddScoped<TextConfigService>()
              //.AddScoped<TwilioService>()
              //.AddScoped<VoiceConfigService>();

            // site-specific policies
            //services
            //  .AddAuthorization(options => {
            //      // application role
            //      options.AddPolicy(Constants.Roles.ConnectUser.NAME, policy => {
            //          policy.Requirements.Add(new ConnectActiveUserRequirement());
            //          policy.RequireRole(Constants.Roles.ConnectUser.NAME);
            //      });

            //      // application role
            //      options.AddPolicy(Constants.Roles.PatientPortalUser.NAME, policy => {
            //          policy.Requirements.Add(new PatientPortalActiveUserRequirement());
            //          policy.RequireRole(Constants.Roles.PatientPortalUser.NAME);
            //      });

            //      // connect roles
            //      options.AddPolicy(Constants.Roles.Manager.NAME, policy => {
            //          policy.Requirements.Add(new ConnectActiveUserRequirement());
            //          policy.RequireRole(Constants.Roles.Manager.NAME);
            //      });

            //      options.AddPolicy(Constants.Roles.Provider.NAME, policy => {
            //          policy.Requirements.Add(new ConnectActiveUserRequirement());
            //          policy.RequireRole(Constants.Roles.Provider.NAME);
            //      });

            //      options.AddPolicy(Constants.Roles.Support.NAME, policy => {
            //          policy.Requirements.Add(new ConnectActiveUserRequirement());
            //          policy.RequireRole(Constants.Roles.Support.NAME);
            //      });

            //      options
              // permissions: billing
              //.AddBatch()
              //.AddGuarantor()
              //// permissions: configuration
              //.AddBillingConfig()
              //.AddChargeSlipConfig()
              //.AddDemographicConfig()
              //.AddDocumentConfig()
              //.AddEpisodeConfig()
              //.AddFormConfig()
              //.AddInsuranceConfig()
              //.AddLetterheadConfig()
              //.AddMessagingConfig()
              //.AddNoteConfig()
              //.AddPatientEducationConfig()
              //.AddPatientInterventionConfig()
              //.AddPatientTrackerConfig()
              //.AddSchedulingConfig()
              //.AddSmartTextConfig()
              //// permissions: modules
              //.AddPatientTracker()
              //// permissions: patients
              //.AddPatient()
              //// permissions: reporting
              //.AddReporting()
              //// permissions: scheduling
              //.AddScheduling()
              //// permissions: tools
              //.AddAddressBook()
              //.AddAppointmentReminder()
              //.AddAuditLog()
              //.AddAutomatedMeasure()
              //.AddChangeHealthcare()
              //.AddDataExport()
              //.AddRcopiaMessages()
              //.AddRcopiaReports()
              //.AddWorklist();
              });

            //services
            //  .AddSingleton<IAuthorizationHandler, ActiveUserHandler>()
            //  .AddSingleton(x => new
            //    CacheService(
            //      x.GetRequiredService<IEnvironmentAccessor>(),
            //      x.GetRequiredService<IWebHostEnvironment>(),
            //      x.GetRequiredService<IHttpContextAccessor>(),
            //      x.GetRequiredService<IMemoryCache>(),
            //      x.GetRequiredService<IServiceScopeFactory>()));
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
