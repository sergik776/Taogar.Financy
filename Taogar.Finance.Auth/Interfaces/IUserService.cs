using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taogar.Finance.Auth.Interfaces
{
    public interface IUserService
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string RequestId { get; set; }
        public void SetUser(string user, string request, string role);
    }
}
