using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fotografolio.Data;
using Fotografolio.Data.Models;
using Fotografolio.Data.Interfaces;
using Fotografolio.Helpers;
using Fotografolio.Services;
using Fotografolio.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VueCliMiddleware;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fotografolio
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        private IHostEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            if (!CurrentEnvironment.IsDevelopment())
                services.AddControllersWithViews().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            else
                services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddHttpContextAccessor();
            services.AddAntiforgery();

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "clientapp";
                });
            }


            if (services.GetHerokuPostgreSQLConnectionString() != null)
            {
                FotografolioDbContext.HerokuPostgreSqlConnectionString = services.GetHerokuPostgreSQLConnectionString();
                services.AddEntityFrameworkNpgsql().AddDbContext<FotografolioDbContext>(opt =>
                    opt.UseNpgsql(services.GetHerokuPostgreSQLConnectionString()));
            }
            else
            {
                FotografolioDbContext.LocalSQLServerConnectionString = Configuration.GetConnectionString("FotografolioContext");
                services.AddDbContext<FotografolioDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("FotografolioContext")));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<FotografolioDbContext>();


            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICloudniaryService, CloudinaryService>();
            services.AddTransient<NpmManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });


            services.AddAuthentication()
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //services.AddAuthentication("CookieAuthentication")
            //.AddCookie("CookieAuthentication", config =>
            //{
            //    config.Cookie.Name = "UserLoginCookie";
            //    config.LoginPath = "/login";
            //    cfg.SlidingExpiration = true;
            //    cfg.ExpireTimeSpan = TimeSpan.FromHours(12);
            //    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            //});

            services.AddAuthorization(options =>
            {
                //var defaultAuthorizationPolicyBuilder = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(
                //    CookieAuthenticationDefaults.AuthenticationScheme,
                //    JwtBearerDefaults.AuthenticationScheme);
                //defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                //options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

                options.AddPolicy(AuthorizationPolicies.JWT, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(AuthorizationPolicies.Cookie, policy =>
                {
                    policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            FotografolioDbContext dbcontext)
        {
            NpmManager.CopyBuildFilesToPublicRoot(env);

            if (FotografolioDbContext.HerokuPostgreSqlConnectionString == null)
                dbcontext.MigrateDatabse("Initial");
            else
                dbcontext.Database.Migrate();

            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            app.UseRouting();
            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            else
                app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "clientapp/dist"))

            //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //{
            //    AuthenticationScheme = "Cookies",
            //    LoginPath = new PathString("/Account/Login/"),
            //    AutomaticAuthenticate = true
            //});
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                if (!CurrentEnvironment.IsDevelopment())
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapFallbackToController("Index", "Home");
                }
                endpoints.MapControllers();

            });

            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    if (env.IsDevelopment())
                        spa.Options.SourcePath = "clientapp";
                    else
                        spa.Options.SourcePath = "dist";

                    if (env.IsDevelopment())
                    {
                        spa.UseVueCli(npmScript: "serve");
                    }

                });
            }

        }
    }
}
