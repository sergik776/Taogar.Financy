using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Domain.Interfaces;

namespace Taogar.Finance.Domain.Models
{
    public class Person : IUserEntity
    {
        public int Id { get; set; }
        public string KeyCloakId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
