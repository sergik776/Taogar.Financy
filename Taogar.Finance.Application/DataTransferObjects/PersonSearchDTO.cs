﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taogar.Finance.Application.DataTransferObjects
{
    public class PersonSearchDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FromCreateDate { get; set; }
        public string ToCreateDate { get; set; }
    }
}
