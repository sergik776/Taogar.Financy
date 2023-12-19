using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Application.DataTransferObjects;
using Taogar.Finance.Application.Extensions;
using Taogar.Finance.Application.Interfaces;
using Taogar.Finance.Database.Interfaces;
using Taogar.Finance.Domain.Models;
using Taogar.HTTP.Domain.Models.Errors;

namespace Taogar.Finance.Application.Services
{
    public class PersonServiceForManagers : IPersonService
    {
        private readonly ILogger logger;
        private readonly IGenericRepository<Person> personRepository;

        public PersonServiceForManagers(Logger.ILogger logger, IGenericRepository<Person> personRepository)
        {
            this.logger = logger;
            this.personRepository = personRepository;
        }

        public Task<PersonInfoDTO> AddPerson(CreatePersonDTO model)
        {
            logger.Info<PersonServiceForManagers>("Добавление клиента, не реализован", ConsoleColor.DarkBlue);
            throw new BaseHTTPError(405, "Метод не реализован");
        }

        public async Task<IEnumerable<PersonInfoDTO>> AllPersons(Expression<Func<PersonSearchDTO, bool>>? predicate = null)
        {
            logger.Info<PersonServiceForManagers>("Вывод всех клиентов", ConsoleColor.DarkBlue);
            return (await personRepository.GetAll()).Select(x=> new PersonInfoDTO()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreateDate = x.CreateDate.ToShortDateString(),
                Email = x.Email,
                Phone = x.Phone
            });
        }

        public async Task<bool> DeletePerson(int id)
        {
            logger.Info<PersonServiceForManagers>("Удаление клиента", ConsoleColor.DarkBlue);
            return await personRepository.Delete(id);
        }

        public async Task<PersonInfoDTO> EditPerson(int id, CreatePersonDTO model)
        {
            logger.Info<PersonServiceForManagers>("Изменение клиента", ConsoleColor.DarkBlue);
            return (await personRepository.Update(new Person()
            {
                Id = id,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone
            })).ToPersonInfoDTO();
        }

        public async Task<PersonInfoDTO> GetPersonById(int id)
        {
            logger.Info<PersonServiceForManagers>("Получение клиента по айди", ConsoleColor.DarkBlue);
            return (await personRepository.GetById(id)).ToPersonInfoDTO();
        }
    }
}
