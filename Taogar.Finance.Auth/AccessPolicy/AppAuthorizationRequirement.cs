using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Auth.Interfaces;
using Taogar.Finance.Auth.Services;
using Taogar.HTTP.Domain.Models.Errors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Taogar.Finance.Auth.AccessPolicy
{
    public class AppAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    public class AppOwnershipAuthorizationHandler : AuthorizationHandler<AppAuthorizationRequirement>
    {
        private readonly AuthConfig config;
        private readonly ILogger logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        public AppOwnershipAuthorizationHandler(AuthConfig _config, ILogger _logger, IHttpContextAccessor _httpContextAccessor, IUserService _userService)
        {
            config = _config;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
            userService = _userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AppAuthorizationRequirement requirement)
        {
            var httpContext = httpContextAccessor.HttpContext;

            logger.Info<AppOwnershipAuthorizationHandler>("===============================================================================================", ConsoleColor.DarkYellow);
            logger.Info<AppOwnershipAuthorizationHandler>("Идентификация пользователя", ConsoleColor.DarkYellow);
            logger.Info<AppOwnershipAuthorizationHandler>($"Путь запроса: {httpContext.Request.Path.ToString()}", ConsoleColor.DarkYellow);
            ClaimsPrincipal? user = ((dynamic)context.Resource).User;
            if (user.Claims.ToList().Count == 0)
            {
                logger.Info<AppOwnershipAuthorizationHandler>("Токен отсутствует\n\n", ConsoleColor.DarkYellow);
                context.Fail();
            }
            else
            {
                var id = user.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").First().Value;
                var Areas = user.Claims.Where(x => x.Type == "resource_access").First().Value;
                string[] roles = GetRoles(Areas, config.AppName);
                if (roles == null || roles.Count() == 0)
                {
                    logger.Info<AppOwnershipAuthorizationHandler>($"User: {id} является членом экосистемы Taogar, но не имеет прав доступа к приложению " +
                        $"{config.AppName}\n\n", ConsoleColor.DarkYellow);

                    context.Fail(new AuthorizationFailureReason(this, $"Вы являетесь членом экосистемы Taogar, но у вас нет прав доступа к приложению {config.AppName}"));
                }
                else
                {
                    logger.Info<AppOwnershipAuthorizationHandler>($"Пользователь идентифицирован: {id}, доступ к {config.AppName} разрешен, Роли: {roles.First()}"
                        , ConsoleColor.DarkYellow);
                    userService.SetUser(id, httpContext.TraceIdentifier, roles.First());
                    context.Succeed(requirement);
                }
            }
            //context.Succeed(requirement);
        }

        private static string[] GetRoles(string jsonString, string key)
        {
            JObject jsonObject = JObject.Parse(jsonString);

            if (jsonObject.TryGetValue(key, out var rolesToken) && rolesToken["roles"] is JArray rolesArray)
            {
                return rolesArray.ToObject<string[]>();
            }

            return Array.Empty<string>();
        }
    }
}
