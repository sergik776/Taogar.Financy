using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Taogar.Finance.Application.DataTransferObjects;
using Taogar.Finance.Application.Interfaces;
using Taogar.Finance.Auth.Filters;
using Taogar.Finance.Auth.Filters.Action;

namespace Taogar.Finance.Presentation.Controllers
{
    [ApiController]
    [Authorize(Policy = "AppOwnershipPolicy")]
    [TypeFilter(typeof(KeyCloakControlerRoleFilter), Arguments = new[] { "User, Manager" })]
    [Route("[controller]")]
    public class PersonController(Logger.ILogger _logger, IPersonService _personService) : ControllerBase
    {
        private readonly Logger.ILogger logger = _logger;
        private readonly IPersonService personService = _personService;

        /// <summary>
        /// Все пользователи (Юзеру возаращает его юзеринфо, менеджеру всех юзеров)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PersonInfoDTO>> AllPersons()
        {
            logger.Info<PersonController>($"Все клиенты", ConsoleColor.Green);
            return await personService.AllPersons();
        }

        /// <summary>
        /// Добавить пользователя (юзеру запрещен, менеджеру разрешен, но правила в токене)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "Manager" })]
        [TypeFilter(typeof(KeyCloakDynamicMethodFilter))]
        public IActionResult AddPerson()
        {
            logger.Info<PersonController>($"Добавить клиента", ConsoleColor.Green);
            return Ok();
        }

        /// <summary>
        /// СВОБОДНО (юзеру запрещен, менеджеру запрещен)
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "God" })]
        public IActionResult EditAllPersons()
        {
            logger.Info<PersonController>($"Изменитьв всх юзеров", ConsoleColor.Green);
            return Ok();
        }

        /// <summary>
        /// Удалить всех пользователей (юзеру запрещен, менеджеру запрещен)
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "God" })]
        public IActionResult RemovePersons()
        {
            logger.Info<PersonController>($"Удалить всех юзеров", ConsoleColor.Green);
            return Ok();
        }








        /// <summary>
        /// Инфо пользователя (юзеру разрешен если айди его, менеджеру разрешен, но правила в токене)
        /// </summary>
        /// <param name="pid">Айди пользователя</param>
        /// <returns></returns>
        [HttpGet("{pid}")]
        [ServiceFilter(typeof(UserToPersonAllowOnlyOwnId))]
        public async Task<PersonInfoDTO> PersonInfo(int pid)
        {
            logger.Info<PersonController>($"информация о клиенте {pid}", ConsoleColor.Green);
            return (await personService.GetPersonById(pid));
        }

        /// <summary>
        /// СВОБОДЕН (юзеру запрещен, менеджеру запрещен)
        /// </summary>
        /// <param name="pid">Айди пользователя</param>
        /// <returns></returns>
        [HttpPost("{pid}")]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "God" })]
        public IActionResult PersonPost(int pid)
        {
            logger.Info<PersonController>($"Пост запрос не реализован {pid}", ConsoleColor.Green);
            return Ok();
        }

        /// <summary>
        /// Изменить пользователя (юзеру разрешен если айди его, менеджеру разрешен, но правила в токене)
        /// </summary>
        /// <param name="pid">Айди пользователя</param>
        /// <returns></returns>
        [HttpPut("{pid}")]
        [ServiceFilter(typeof(UserToPersonAllowOnlyOwnId))]
        public IActionResult EditPerson(int pid)
        {
            logger.Info<PersonController>($"Изменить клиента {pid}", ConsoleColor.Green);
            return Ok();
        }

        /// <summary>
        /// Удалить пользователя  (юзеру запрещен, менеджеру разрешен, но правила в токене)
        /// </summary>
        /// <param name="pid">Айди пользоваетля</param>
        /// <returns></returns>
        [HttpDelete("{pid}")]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "Manager" })]
        [TypeFilter(typeof(KeyCloakDynamicMethodFilter))]
        public IActionResult RemovePerson(int pid)
        {
            logger.Info<PersonController>($"Удалить клиента {pid}", ConsoleColor.Green);
            return Ok();
        }
    }
}
