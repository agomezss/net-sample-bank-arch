using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Bank.Infra.CrossCutting.IoC;
using Bank.Core.Models;
using Bank.Services.Authentication;
using Bank.Api.Logging;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.Swagger;

namespace Bank.Services.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Hosting { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
            Hosting = env;

            BankApiSettings.Configure(Configuration);

            BankTypes.Initialize(env);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authConnectionString = Configuration.GetConnectionString(Configuration["ActiveDbConnection"]);

            services.AddDbContext<AuthDbContext>(cfg =>
            {
                cfg.UseSqlServer(authConnectionString, m => m.MigrationsAssembly("BankApi"));
            });
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services
              .AddIdentity<BankUser, IdentityRole>(cfg =>
              {
                  cfg.User.RequireUniqueEmail = true;
                  cfg.Password.RequireDigit = true;
                  cfg.Password.RequiredLength = 6;
                  cfg.Password.RequiredUniqueChars = 0;
                  cfg.Password.RequireLowercase = false;
                  cfg.Password.RequireNonAlphanumeric = false;
                  cfg.Password.RequireUppercase = false;
              })
              .AddEntityFrameworkStores<AuthDbContext>();

            services
              .AddAuthentication(options =>
              {
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

              })
              .AddCookie()
              .AddJwtBearer(cfg =>
              {
                  // This handler changes the claim names, this reverts it:

                  cfg.RequireHttpsMetadata = false; 
                  cfg.SaveToken = true;

                  cfg.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidIssuer = Configuration["Tokens:Issuer"],
                      ValidAudience = Configuration["Tokens:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                  };

                  cfg.Events = new JwtBearerEvents()
                  {
                        OnAuthenticationFailed = (fContext) =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = (challenge) =>
                        {
                            var u = challenge.HttpContext.User;

                            var challengeResult = challenge.AuthenticateFailure;

                            return Task.CompletedTask;
                        },
                        OnMessageReceived = (msg) =>
                        {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = (token) =>
                        {
                            var identity = token.Principal.Identity as ClaimsIdentity;

                            token.HttpContext.User = token.Principal;

                            return Task.CompletedTask;
                        }
                  };
              });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddMvc(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", null);
            });

            // .NET Native DI Abstraction
            RegisterServices(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseCors("AllowAll");

            //app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute("DefaultApi", "api/{controller}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank API v1.0");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(provider => Configuration);
            services.AddSingleton(provider => Hosting);

            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
