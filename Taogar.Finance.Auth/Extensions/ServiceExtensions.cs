using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using Taogar.Finance.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Taogar.Finance.Auth.AccessPolicy;
using Taogar.Finance.Auth.Middlewares;
using Taogar.Finance.Auth.Interfaces;
using Taogar.Finance.Auth.Filters.Action;

namespace Taogar.Finance.Auth.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddKeyCloakAuthentication(this IServiceCollection services)
        {
            var config = new AuthConfig();
            services.AddSingleton(config);

            var certificate = new X509Certificate2(Encoding.UTF8.GetBytes(config.Certificate));
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config.Issuer,
                    ValidateAudience = true,
                    ValidAudience = config.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new X509SecurityKey(certificate), // Используем сертификат для валидации
                    ClockSkew = TimeSpan.Zero // Не учитываем разницу во времени (можно установить другое значение)
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AppOwnershipPolicy", policy =>
                    policy.Requirements.Add(new AppAuthorizationRequirement()));
            });
            services.AddScoped<IAuthorizationHandler, AppOwnershipAuthorizationHandler>();
            //services.AddScoped<IUserService, UserService>(); //Добавляем сервис пользователя
            services.AddSingleton<IAuthorizationMiddlewareResultHandler,
                          MyAuthorizationMiddlewareResultHandler>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserToPersonAllowOnlyOwnId>();
            services.AddScoped<IKeyCloakService, KeyCloakService>();
            return services;
        }
    }
}
