using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Application.DataTransferObjects;
using Taogar.Finance.Application.Extensions;
using Taogar.Finance.Application.Interfaces;
using Taogar.Finance.Auth.Interfaces;
using Taogar.Finance.Database.Interfaces;
using Taogar.Finance.Domain.Models;
using Taogar.HTTP.Domain.Models.Errors;

namespace Taogar.Finance.Application.Services
{
    public class PersonServiceForUser : IPersonService
    {
        private readonly Logger.ILogger logger;
        private readonly IGenericRepository<Person> personRepository;
        private readonly IUserService userService;

        public PersonServiceForUser(Logger.ILogger logger, IGenericRepository<Person> personRepository, IUserService _userService)
        {
            this.logger = logger;
            this.personRepository = personRepository;
            userService = _userService;
        }

        public Task<PersonInfoDTO> AddPerson(CreatePersonDTO model)
        {
            logger.Info<PersonServiceForUser>("Нет прав доступа для клиента", ConsoleColor.DarkBlue);
            throw new BaseHTTPError(423, "Не достаточно прав доступа");
        }

        public async Task<IEnumerable<PersonInfoDTO>> AllPersons(Expression<Func<PersonSearchDTO, bool>>? predicate = null)
        {
            logger.Info<PersonServiceForUser>("Запрос всех клиентов, вернется только собственный обьект для юзера", ConsoleColor.DarkBlue);
            return (await personRepository.GetAll(x => x.KeyCloakId == userService.UserId)).Select(x => new PersonInfoDTO()
            {
                CreateDate = x.CreateDate.ToShortDateString(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Id = x.Id,
                Phone = x.Phone
            });
        }

        public Task<bool> DeletePerson(int id)
        {
            logger.Info<PersonServiceForUser>("Нет прав доступа для клиента", ConsoleColor.DarkBlue);
            throw new BaseHTTPError(423, "Не достаточно прав доступа");
        }

        public async Task<PersonInfoDTO> EditPerson(int id, CreatePersonDTO model)
        {
            logger.Info<PersonServiceForUser>("Изменить клиента", ConsoleColor.DarkBlue);
            return (await personRepository.Update(model.ToPerson(id))).ToPersonInfoDTO();
        }

        public async Task<PersonInfoDTO> GetPersonById(int id)
        {
            logger.Info<PersonServiceForUser>("Получить клиета по айди", ConsoleColor.DarkBlue);
            return (await personRepository.GetById(id)).ToPersonInfoDTO();
        }
    }
}
