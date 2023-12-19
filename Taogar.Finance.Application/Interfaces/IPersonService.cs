using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Application.DataTransferObjects;

namespace Taogar.Finance.Application.Interfaces
{
    public interface IPersonService
    {
        Task<PersonInfoDTO> GetPersonById(int id);
        Task<PersonInfoDTO> AddPerson(CreatePersonDTO model);
        Task<bool> DeletePerson(int id);
        Task<PersonInfoDTO> EditPerson(int id, CreatePersonDTO model);
        Task<IEnumerable<PersonInfoDTO>> AllPersons(Expression<Func<PersonSearchDTO, bool>>? predicate = null);
    }
}
