using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Auth.Interfaces;

namespace Taogar.Finance.Auth.Services
{
    public class UserService : IUserService
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string RequestId { get; set; }

        Logger.ILogger logger;

        public UserService(Logger.ILogger _logger)
        {
            UserId = string.Empty; 
            Role = string.Empty; 
            RequestId = string.Empty;
            logger = _logger;
        }

        public void SetUser(string _userId, string request, string role)
        {
            UserId = _userId;
            RequestId = request;
            Role = role;
            logger.Info<UserService>($"Пользователь идентифицирован {UserId}, Роль: {Role}, Ключ запроса: {RequestId}", ConsoleColor.Blue);
        }
    }
}
