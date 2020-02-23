using IdentityServer4.Services;
using IdServer.Controllers;
using IdServer.Data;
using IdServer.Hubs;
using IdServer.Models;
using IdServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://localhost:44320",
                        "http://localhost:4200", // TODO: change to https
                        "http://localhost:5000",
                        "http://localhost:5001")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddControllersWithViews();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.Audience = "comfyDotApi";
                });
            services.AddLocalApiAuthentication();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddScoped<IRoomController, RoomController>();
            services.AddSingleton<INameDataService, NameDataService>();
            services.AddSingleton<INameGenerator, NameGeneratorService>();
            services.AddSingleton<IPasswordDataService, PasswordDataService>();
            services.AddSingleton<IPasswordGenerator, PasswordGeneratorService>();
            services.AddSingleton<IColorGenerator, ColorGenerator>();

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ComfyDotDB"));
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.Cors.CorsPaths.Add(new PathString("/Account/Register"));
                    options.Cors.CorsPaths.Add(new PathString("/Account/Login"));
                    options.Cors.CorsPaths.Add(new PathString("/api/rooms"));
                    options.Cors.CorsPaths.Add(new PathString("/chathub"));
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<User>();

            // Not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
            // Use your own certificate here
            // builder.AddSigningCredential();

            services.AddAuthentication(IdentityConstants.ApplicationScheme);

            services.AddScoped<IProfileService, ProfileService>();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });
            app.UseCors("default");
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}