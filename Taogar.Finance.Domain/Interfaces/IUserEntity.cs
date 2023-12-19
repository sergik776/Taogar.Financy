using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taogar.Finance.Domain.Interfaces
{
    public interface IUserEntity : IEntity
    {
        public string KeyCloakId { get; set; }
    }
}
