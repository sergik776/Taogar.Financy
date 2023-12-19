using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
        /// ��� ������������ (����� ���������� ��� ��������, ��������� ���� ������)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PersonInfoDTO>> AllPersons()
        {
            logger.Info<PersonController>($"��� �������", ConsoleColor.Green);
            return await personService.AllPersons();
        }

        /// <summary>
        /// �������� ������������ (����� ��������, ��������� ��������, �� ������� � ������)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "Manager" })]
        [TypeFilter(typeof(KeyCloakDynamicMethodFilter))]
        public async Task<IActionResult> AddPerson([FromBody] CreatePersonDTO model)
        {
            logger.Info<PersonController>($"�������� �������", ConsoleColor.Green);
            await personService.AddPerson(model);
            return Created();
        }

        /// <summary>
        /// �������� (����� ��������, ��������� ��������)
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "God" })]
        public IActionResult EditAllPersons()
        {
            logger.Info<PersonController>($"��������� ��� ������", ConsoleColor.Green);
            return Ok();
        }

        /// <summary>
        /// ������� ���� ������������� (����� ��������, ��������� ��������)
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "God" })]
        public IActionResult RemovePersons()
        {
            logger.Info<PersonController>($"������� ���� ������", ConsoleColor.Green);
            return Ok();
        }








        /// <summary>
        /// ���� ������������ (����� �������� ���� ���� ���, ��������� ��������, �� ������� � ������)
        /// </summary>
        /// <param name="pid">���� ������������</param>
        /// <returns></returns>
        [HttpGet("{pid}")]
        [ServiceFilter(typeof(UserToPersonAllowOnlyOwnId))]
        public async Task<PersonInfoDTO> PersonInfo(int pid)
        {
            logger.Info<PersonController>($"���������� � ������� {pid}", ConsoleColor.Green);
            return (await personService.GetPersonById(pid));
        }

        /// <summary>
        /// �������� (����� ��������, ��������� ��������)
        /// </summary>
        /// <param name="pid">���� ������������</param>
        /// <returns></returns>
        [HttpPost("{pid}")]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "God" })]
        public IActionResult PersonPost(int pid)
        {
            logger.Info<PersonController>($"���� ������ �� ���������� {pid}", ConsoleColor.Green);
            return Ok();
        }

        /// <summary>
        /// �������� ������������ (����� �������� ���� ���� ���, ��������� ��������, �� ������� � ������)
        /// </summary>
        /// <param name="pid">���� ������������</param>
        /// <returns></returns>
        [HttpPut("{pid}")]
        [ServiceFilter(typeof(UserToPersonAllowOnlyOwnId))]
        public IActionResult EditPerson(int pid)
        {
            logger.Info<PersonController>($"�������� ������� {pid}", ConsoleColor.Green);
            return Ok();
        }

        /// <summary>
        /// ������� ������������  (����� ��������, ��������� ��������, �� ������� � ������)
        /// </summary>
        /// <param name="pid">���� ������������</param>
        /// <returns></returns>
        [HttpDelete("{pid}")]
        [TypeFilter(typeof(KeyCloakMethodRoleFilter), Arguments = new[] { "Manager" })]
        [TypeFilter(typeof(KeyCloakDynamicMethodFilter))]
        public IActionResult RemovePerson(int pid)
        {
            logger.Info<PersonController>($"������� ������� {pid}", ConsoleColor.Green);
            return Ok();
        }
    }
}
