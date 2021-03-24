using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using DepsWebApp.Clients;
using DepsWebApp.Options;
using DepsWebApp.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.IO;
using DepsWebApp.Middlewares;
using DepsWebApp.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DepsWebApp
{
#pragma warning disable CS1591
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<MigrationService>();
        
            // Add options
            services
                .Configure<CacheOptions>(Configuration.GetSection("Cache"))
                .Configure<NbuClientOptions>(Configuration.GetSection("Client"))
                .Configure<RatesOptions>(Configuration.GetSection("Rates"));

            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("connectionString")), ServiceLifetime.Transient);

            services.AddTransient<IAuthService,AuthService>();
            services.AddScoped<IRatesService, RatesService>();

            services.AddAuthentication(CustomAuthSchema.Name).AddScheme<CustomAuthSchemaOptions, CustomAuthSchemaHandler>(CustomAuthSchema.Name,CustomAuthSchema.Name,null);

            // Add NbuClient as Transient
            services.AddHttpClient<IRatesProviderClient, NbuClient>()
                .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(10));

            // Add CacheHostedService as Singleton
            services.AddHostedService<CacheHostedService>();


           // Add batch of Swashbuckle Swagger services
           var documenation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

           var documentationPath = Path.Combine(AppContext.BaseDirectory, documenation);

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Id = "EncryptedAccount",
                                        Type = ReferenceType.SecurityScheme
                                    },
                                },
                                new string[0]
                            }
                        });

                c.AddSecurityDefinition(
                    "EncryptedAccount",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Scheme = "Base64",
                        Name = "Basic Authorization",
                        Description = "Base64 Code",
                        BearerFormat = "SessionId"
                    });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DI Demo App API", Version = "v1" });

                if (File.Exists(Path.Combine(AppContext.BaseDirectory, documenation)))
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, documenation), includeControllerXmlComments: true);
            });

            // Add batch of framework services
            services.AddMemoryCache();
            services.AddControllers();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DI Demo App API v1"));
            }

            app.UseMiddleware<LoggerMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
#pragma warning restore CS1591

}
