using Logger;
using Microsoft.IdentityModel.Tokens;
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
    public class PersonServiceForManagers : IPersonService
    {
        private readonly ILogger logger;
        private readonly IGenericRepository<Person> personRepository;
        private readonly IKeyCloakService keyCloakService;

        public PersonServiceForManagers(Logger.ILogger logger, IGenericRepository<Person> personRepository, IKeyCloakService _keyCloakService)
        {
            this.logger = logger;
            this.personRepository = personRepository;
            keyCloakService = _keyCloakService;
        }

        public async Task<PersonInfoDTO> AddPerson(CreatePersonDTO model)
        {
            logger.Info<PersonServiceForManagers>("Добавление клиента, не реализован", ConsoleColor.DarkBlue);

            var createStatus = await keyCloakService.CreateKeyCloakUser(model.FirstName, model.LastName, model.FirstName + model.LastName, model.Email);
            if(createStatus == (System.Net.HttpStatusCode)409)
            {
                throw new BaseHTTPError(409, "Пользователь с такими параметрами уже существует");
            }
            else if(createStatus != (System.Net.HttpStatusCode)201)
            {
                throw new Exception($"При создании аккаунта произошла ошибка, код {createStatus}");
            }

            var userId = await keyCloakService.GetIdByParams(model.FirstName, model.LastName, model.Email);
            if(userId.IsNullOrEmpty())
            {
                throw new Exception("Пользователь создан, но айди не найден");
            }

            var assignRoleStatus = await keyCloakService.AssingRoleToUser(model.FirstName, model.LastName, model.Email, "User");
            if (assignRoleStatus != (System.Net.HttpStatusCode)204)
            {
                logger.Error<PersonServiceForManagers>($"При создании клиента ({userId}) роль (User) не добавилась, добавить вручную", ConsoleColor.DarkBlue);
            }

            var result = await personRepository.Create(new Person()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                KeyCloakId = userId,
                CreateDate = DateTime.UtcNow,
            });

            return new PersonInfoDTO
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Phone = result.Phone,
                CreateDate = result.CreateDate.ToShortDateString(),
                Email = result.Email
            };
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
