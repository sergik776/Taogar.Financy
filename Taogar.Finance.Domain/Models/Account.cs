using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Domain.Interfaces;

namespace Taogar.Finance.Domain.Models
{
    public class Account : IEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public decimal Balance { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
