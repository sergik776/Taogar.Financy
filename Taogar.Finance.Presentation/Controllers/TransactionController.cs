//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Taogar.Finance.Auth.Filters;

//namespace Taogar.Finance.Presentation.Controllers
//{
//    [ApiController]
//    [Authorize(Policy = "AppOwnershipPolicy")]
//    [TypeFilter(typeof(KeyCloakRoleFilter), Arguments = new[] { "Manager" })]
//    [Route("[controller]")]
//    public class TransactionController : ControllerBase
//    {

//        private readonly ILogger<TransactionController> _logger;

//        public TransactionController(ILogger<TransactionController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok();
//        }
//    }
//}
