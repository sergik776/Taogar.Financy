using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Net;
using Taogar.Finance.Auth.Interfaces;

namespace Taogar.Finance.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevelopmentController : ControllerBase
    {
        private readonly EndpointDataSource _endpointDataSource;
        private readonly IKeyCloakService keyCloakService;
        private readonly Logger.ILogger logger;

        public DevelopmentController(EndpointDataSource endpointDataSource, IKeyCloakService _keyCloakService, Logger.ILogger _logger)
        {
            _endpointDataSource = endpointDataSource;
            keyCloakService = _keyCloakService;
            logger = _logger;
        }

        [HttpGet]
        public async Task<string> EndPoints([FromQuery] string firstName, string lastName, string email)
        {
            logger.Info<DevelopmentController>("Запрос юзер айди из кейклок");
            return await keyCloakService.GetIdByParams(firstName, lastName, email);
        }

        [HttpGet("AddRole")]
        public async Task<string> AddRole([FromQuery] string firstName, string lastName, string email, string rolename)
        {
            logger.Info<DevelopmentController>("Запрос добавления роли");
            return (await keyCloakService.AssingRoleToUser(firstName, lastName, email, rolename)).ToString();
        }

        [HttpGet("AddUser")]
        public async Task<HttpStatusCode> CreateKeyCloakUser([FromQuery] string _firstName, string _lastName, string _userName, string _email)
        {
            logger.Info<DevelopmentController>("Запрос добавления юзера");
            await keyCloakService.CreateKeyCloakUser(_firstName, _lastName, _userName, _email);
            return await keyCloakService.AssingRoleToUser(_firstName, _lastName, _email, "User");
        }
    }
}
