//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Taogar.Finance.Auth.Filters;

//namespace Taogar.Finance.Presentation.Controllers
//{
//    [ApiController]
//    [Authorize(Policy = "AppOwnershipPolicy")]
//    [TypeFilter(typeof(KeyCloakRoleFilter), Arguments = new[] { "Manager" })]
//    [Route("Person/{pid}/[controller]")]
//    public class AccountController : ControllerBase
//    {

//        private readonly ILogger<AccountController> _logger;

//        public AccountController(ILogger<AccountController> logger)
//        {
//            _logger = logger;
//        }

//        /// <summary>
//        /// ��� ����� ������������
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <returns></returns>
//        [HttpGet]
//        public IActionResult AllAccounts(int pid)
//        {
//            return Ok();
//        }

//        /// <summary>
//        /// �������� ���� ������������
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <returns></returns>
//        [HttpPost]
//        public IActionResult AddAccount(int pid)
//        {
//            return Ok();
//        }

//        /// <summary>
//        /// ��������
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <returns></returns>
//        [HttpPut]
//        public IActionResult EditAllAccounts(int pid)
//        {
//            return Ok();
//        }

//        /// <summary>
//        /// ������� ��� ����� ������������
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <returns></returns>
//        [HttpDelete]
//        public IActionResult RemoveAllAccounts(int pid)
//        {
//            return Ok();
//        }



//        /// <summary>
//        /// ���������� � �����
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <param name="aid">���� �����</param>
//        /// <returns></returns>
//        [HttpGet("{aid}")]
//        public IActionResult AccountInfo(int pid, int aid)
//        {
//            return Ok();
//        }

//        /// <summary>
//        /// ��������
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <param name="aid">���� �����</param>
//        /// <returns></returns>
//        [HttpPost("{aid}")]
//        public IActionResult AccountPost(int pid, int aid)
//        {
//            return Ok();
//        }

//        /// <summary>
//        /// ������������� ���� ������������
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <param name="aid">���� �����</param>
//        /// <returns></returns>
//        [HttpPut("{aid}")]
//        public IActionResult EditAccount(int pid, int aid)
//        {
//            return Ok();
//        }

//        /// <summary>
//        /// ������� ���� ������������
//        /// </summary>
//        /// <param name="pid">���� ������������</param>
//        /// <param name="aid">���� �����</param>
//        /// <returns></returns>
//        [HttpDelete("{aid}")]
//        public IActionResult RemoveAccount(int pid, int aid)
//        {
//            return Ok();
//        }
//    }
//}
