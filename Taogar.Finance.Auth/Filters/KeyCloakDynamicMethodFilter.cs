using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Auth.Services;
using Taogar.HTTP.Domain.Models.Errors;

namespace Taogar.Finance.Auth.Filters
{
    public class KeyCloakDynamicMethodFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var q = context.HttpContext.Request.RouteValues["action"].ToString();
            var logger = (Logger.ILogger)context.HttpContext.RequestServices.GetService(typeof(Logger.ILogger));
            var config = (AuthConfig)context.HttpContext.RequestServices.GetService(typeof(AuthConfig));
            logger.Info<KeyCloakDynamicMethodFilter>("проверка прав доступа из токена (динамический)", ConsoleColor.DarkYellow);
            if (!UserHasPermission(context.HttpContext.User, config, q))
            {
                logger.Info<KeyCloakMethodRoleFilter>("доступ к методу запрещен", ConsoleColor.DarkYellow);
                throw new BaseHTTPError(403, "У вас недостаточно прав доступа к этой сущности или операции");
            }
            else
            {
                logger.Info<KeyCloakDynamicMethodFilter>("доступ к методу разрешен", ConsoleColor.DarkYellow);
            }
        }

        private bool UserHasPermission(System.Security.Claims.ClaimsPrincipal user, AuthConfig config, string method)
        {
            var Areas = user.Claims.Where(x => x.Type == "methods").First().Value;
            var methods = Areas.Replace(" ", "").Split(',');
            if (methods.Any(x=>x == method))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
