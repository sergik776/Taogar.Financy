using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Taogar.Finance.Auth.Interfaces
{
    public interface IKeyCloakService
    {
        public Task<string> GetIdByParams(string firstName, string lastName, string email);
        public Task<HttpStatusCode> AssingRoleToUser(string firstName, string lastName, string email, string roleName);
    }
}
