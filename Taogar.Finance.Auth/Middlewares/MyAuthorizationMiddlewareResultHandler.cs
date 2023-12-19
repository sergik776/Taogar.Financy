using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Taogar.Finance.Auth.AccessPolicy;
using Taogar.HTTP.Domain.Models.Errors;

namespace Taogar.Finance.Auth.Middlewares
{
    public class MyAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler
             DefaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(
            RequestDelegate requestDelegate,
            HttpContext context,
            AuthorizationPolicy authorizationPolicy,
            PolicyAuthorizationResult policyAuthorizationResult)
        {
            if (Show404ForForbiddenResult(policyAuthorizationResult))
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 403;

                await context.Response.WriteAsync(new BaseHTTPError(403, policyAuthorizationResult.AuthorizationFailure.FailureReasons.FirstOrDefault().Message).ToString());
                return;
            }
            await DefaultHandler.HandleAsync(requestDelegate, context, authorizationPolicy,
                                   policyAuthorizationResult);
        }

        bool Show404ForForbiddenResult(PolicyAuthorizationResult policyAuthorizationResult)
        {
            if (policyAuthorizationResult.AuthorizationFailure?.FailureReasons?.FirstOrDefault() != null)
            {
                if (policyAuthorizationResult.AuthorizationFailure.FailureReasons.FirstOrDefault().Handler is AppOwnershipAuthorizationHandler)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
