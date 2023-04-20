using HackMyHabit.Domain.Users.Commons;
using HackMyHabit.WebApi.Commons;
using HackMyHabit.WebApi.Consts;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace HackMyHabit.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection SetupAuth(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddOptions<AuthOptions>().Bind(config.GetSection("Auth")).ValidateDataAnnotations();
            services.AddOptions<CookieOptions>().Bind(config.GetSection("Auth:Cookie")).ValidateDataAnnotations();

            services.AddSingleton<IUsersSessionsStorage, UsersSessionsStorage>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationHandler, SessionCheckHandler>();

            services.AddAuthorization(o =>
            {
                o.AddPolicy(AuthConsts.SESSION_CHECK_POLICY, x => x.AddRequirements(new SessionCheckRequirement()));
            });

            var options = new AuthOptions();
            config.GetSection("Auth").Bind(options);

            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireAudience = options.RequireAudience,
                ValidIssuer = options.ValidIssuer,
                ValidIssuers = options.ValidIssuers,
                ValidateActor = options.ValidateActor,
                ValidAudience = options.ValidAudience,
                ValidAudiences = options.ValidAudiences,
                ValidateAudience = options.ValidateAudience,
                ValidateIssuer = options.ValidateIssuer,
                ValidateLifetime = options.ValidateLifetime,
                ValidateTokenReplay = options.ValidateTokenReplay,
                ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
                SaveSigninToken = options.SaveSigninToken,
                RequireExpirationTime = options.RequireExpirationTime,
                RequireSignedTokens = options.RequireSignedTokens,
                ClockSkew = TimeSpan.Zero
            };

            if (string.IsNullOrWhiteSpace(options.IssuerSigningKey))
            {
                throw new ArgumentException("Missing issuer signing key.", nameof(options.IssuerSigningKey));
            }

            if (!string.IsNullOrWhiteSpace(options.AuthenticationType))
            {
                tokenValidationParameters.AuthenticationType = options.AuthenticationType;
            }

            var rawKey = Encoding.UTF8.GetBytes(options.IssuerSigningKey);
            tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(rawKey);

            if (!string.IsNullOrWhiteSpace(options.NameClaimType))
            {
                tokenValidationParameters.NameClaimType = options.NameClaimType;
            }

            if (!string.IsNullOrWhiteSpace(options.RoleClaimType))
            {
                tokenValidationParameters.RoleClaimType = options.RoleClaimType;
            }

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = options.Authority;
                o.Audience = options.Audience;
                o.MetadataAddress = options.MetadataAddress;
                o.SaveToken = options.SaveToken;
                o.RefreshOnIssuerKeyNotFound = options.RefreshOnIssuerKeyNotFound;
                o.RequireHttpsMetadata = options.RequireHttpsMetadata;
                o.IncludeErrorDetails = options.IncludeErrorDetails;
                o.TokenValidationParameters = tokenValidationParameters;
                if (!string.IsNullOrWhiteSpace(options.Challenge))
                {
                    o.Challenge = options.Challenge;
                }

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue(AuthConsts.ACCESS_TOKEN_COOKIE, out var token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                };
            });

            return services;
        }

        public static IServiceCollection SetupModules(this IServiceCollection services, IConfiguration configuration)
        {
            //services
                //.AddSharedInsfrastructureServices(configuration)
                //.AddUsersServices()

            return services;
        }

        public static IServiceCollection SetupMediatr(this IServiceCollection services)
        {
            var assemblies = GetAssemblies().ToArray();
            services.AddMediatR(assemblies);

            return services;
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();

            var entryAssembly = Assembly.GetAssembly(typeof(Program))!;
            stack.Push(entryAssembly);
            do
            {
                var asm = stack.Pop();
                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                    if (!list.Contains(reference.FullName) && reference.FullName.StartsWith("HackMyHabit"))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }

            }
            while (stack.Count > 0);
        }
    }
}
