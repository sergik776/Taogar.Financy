using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Auth.Interfaces;
using Taogar.Finance.Database.Interfaces;
using Taogar.Finance.Domain.Interfaces;
using Taogar.Finance.Domain.Models;
using Taogar.HTTP.Domain.Models.Errors;

namespace Taogar.Finance.Auth.Filters.Action
{
    public class UserToPersonAllowOnlyOwnId : ActionFilterAttribute
    {
        private readonly IGenericRepository<Person> repository; // Замените на ваш интерфейс репозитория
        private readonly IUserService userService;
        Logger.ILogger logger;

        public UserToPersonAllowOnlyOwnId(IGenericRepository<Person> _repository, IUserService _userService, Logger.ILogger _logger)
        {
            this.repository = _repository;
            this.userService = _userService;
            logger = _logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (userService.Role == "User")
            {
                logger.Info<UserToPersonAllowOnlyOwnId>("Проверка: принадлежит ли запрашиваемый Id клиенту", ConsoleColor.Yellow);
            
                if (context.ActionArguments.ContainsKey("pid"))
                {
                    var id = int.Parse(context.ActionArguments["pid"].ToString());
                    var entity = repository.IsEntityExist(id, x => x.KeyCloakId == userService.UserId).Result;

                    if (!entity)
                    {
                        logger.Info<UserToPersonAllowOnlyOwnId>("Не принадлежит", ConsoleColor.Yellow);
                        throw new BaseHTTPError(403, "У вас недостаточно прав доступа к этой сущности или операции");
                    }
                }
                logger.Info<UserToPersonAllowOnlyOwnId>("Принадлежит", ConsoleColor.Yellow);
            }
            logger.Info<UserToPersonAllowOnlyOwnId>("Запрашивает не User", ConsoleColor.Yellow);
            base.OnActionExecuting(context);
        }
    }
}
