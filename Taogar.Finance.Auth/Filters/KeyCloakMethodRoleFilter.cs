using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Auth.Services;
using Taogar.HTTP.Domain.Models.Errors;

namespace Taogar.Finance.Auth.Filters
{
    public class KeyCloakMethodRoleFilter : IAuthorizationFilter
    {
        private string[] Roles { get; set; }

        public KeyCloakMethodRoleFilter()
        {

        }

        public KeyCloakMethodRoleFilter(string roles)
        {
            Roles = roles.Replace(" ", "").Split(',');
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var logger = (Logger.ILogger)context.HttpContext.RequestServices.GetService(typeof(Logger.ILogger));
            var config = (AuthConfig)context.HttpContext.RequestServices.GetService(typeof(AuthConfig));
            logger.Info<KeyCloakMethodRoleFilter>("проверка роли пользователя", ConsoleColor.DarkYellow);
            if (!UserHasPermission(context.HttpContext.User, config))
            {
                logger.Info<KeyCloakMethodRoleFilter>("доступ к методу запрещен", ConsoleColor.DarkYellow);
                throw new BaseHTTPError(403, "У вас недостаточно прав доступа к этой сущности или операции");
            }
            else
            {
                logger.Info<KeyCloakMethodRoleFilter>("доступ к методу разрешен", ConsoleColor.DarkYellow);
            }
        }

        private bool UserHasPermission(System.Security.Claims.ClaimsPrincipal user, AuthConfig config)
        {
            var Areas = user.Claims.Where(x => x.Type == "resource_access").First().Value;
            var roles = GetRoles(Areas, config.AppName);
            if (roles.Intersect(Roles).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
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
