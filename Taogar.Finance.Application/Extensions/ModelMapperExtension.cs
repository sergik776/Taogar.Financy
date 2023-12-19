using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Application.DataTransferObjects;
using Taogar.Finance.Domain.Models;

namespace Taogar.Finance.Application.Extensions
{
    public static class ModelMapperExtension
    {
        public static Person ToPerson(this CreatePersonDTO model, int id)
        {
            if (model == null) return null;
            return new Person()
            {
                Id = id,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone
            };
        }

        public static PersonInfoDTO ToPersonInfoDTO(this Person res)
        {
            if (res == null) return null;
            return new PersonInfoDTO()
            {
                Phone = res.Phone,
                FirstName = res.FirstName,
                LastName = res.LastName,
                Email = res.Email,
                Id = res.Id,
                CreateDate = res.CreateDate.ToShortDateString()
            };
        }
    }
}
