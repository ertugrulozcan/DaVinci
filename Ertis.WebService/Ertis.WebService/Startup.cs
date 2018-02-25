using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Ertis.WebService.Auth;
using Ertis.WebService.Dao.Contracts;
using Ertis.WebService.Dao.MySql;
using Ertis.WebService.Extensions;
using Ertis.WebService.Services;
using Ertis.WebService.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Ertis.WebService
{
    public partial class Startup
    {
        public readonly static bool SslRequired = true;

        public JWTSettings DefaultJwtSettings { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"Localization/Data/tr-TR.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"Localization/Data/en-US.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds services required for using options.
            services.AddOptions();

            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<Dao.MySql.ConnectionString>(Configuration.GetSection("ConnectionString"));
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));

            var sp = services.BuildServiceProvider();
            var jwtOptionsAccessor = sp.GetService<IOptions<JWTSettings>>();
            this.DefaultJwtSettings = jwtOptionsAccessor.Value;

            services.AddCors(option =>
            {
                option.AddPolicy("AllowSpecificOrigin", policy => policy.AllowAnyOrigin());
                option.AddPolicy("AllowGetMethod", policy => policy.AllowAnyMethod());
            });

            // Add framework services.
            services.AddMvc(options =>
            {

            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));

                if (SslRequired)
                    options.Filters.Add(new RequireHttpsAttribute());
            });

            // Add application services.
            services.AddSingleton<ICredentialsRepository, CredentialsRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IStaffRepository, StaffRepository>();
            services.AddSingleton<IOrganizationRepository, OrganizationRepository>();

            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IOrganizationService, OrganizationService>();

            this.ConfigureAuth(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Localization.LocalizationManager.Current.LoadFromResource(env, "tr-TR");
            Localization.LocalizationManager.Current.LoadFromResource(env, "en-US");

            app.UseExceptionHandler(appBuilder => this.SetUnhandledExceptionHandler(appBuilder));
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseCors("AllowSpecificOrigin");
            app.UseTokenProvider(this.GenerateTokenProviderOptions());

            app.UseAuthentication();

            // SSL
            if (SslRequired)
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
            }

            app.UseMvc();
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            Ertis.WebService.Services.ServiceProvider.Create(services.BuildServiceProvider());

            var tokenValidationParameters = this.GenerateTokenValidationParameters();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = this.DefaultJwtSettings.CookieName;
                options.TicketDataFormat = new CustomJwtDataFormat(SecurityAlgorithms.HmacSha256, tokenValidationParameters);
            });
        }

        private TokenValidationParameters GenerateTokenValidationParameters()
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Helpers.SignatureHelper.GenerateSigningKey(this.DefaultJwtSettings.SecretKey),

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = this.DefaultJwtSettings.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = this.DefaultJwtSettings.Audience,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            return tokenValidationParameters;
        }

        private TokenProviderOptions GenerateTokenProviderOptions()
        {
            var tokenProviderOptions = new TokenProviderOptions()
            {
                Path = this.DefaultJwtSettings.Path,
                Audience = this.DefaultJwtSettings.Audience,
                Issuer = this.DefaultJwtSettings.Issuer,
                SigningCredentials = Helpers.SignatureHelper.GenerateSigningCredentials(this.DefaultJwtSettings.SecretKey),
                IdentityResolver = GetIdentity
            };

            return tokenProviderOptions;
        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            return Ertis.WebService.Services.ServiceProvider.Current.AuthenticationService.GetIdentity(username, password);
        }

        private void SetUnhandledExceptionHandler(IApplicationBuilder appBuilder)
        {
            appBuilder.Use(async (context, next) =>
            {
                var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                //when authorization has failed, should retrun a json message to client
                if (error != null && error.Error is SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        State = "Unauthorized",
                        Msg = "Token expired!"
                    }));
                }
                //when orther error, retrun a error message json to client
                else if (error != null && error.Error != null)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        State = "Internal Server Error",
                        Msg = error.Error.Message
                    }));
                }
                //when no error, do next.
                else await next();
            });
        }
    }
}
