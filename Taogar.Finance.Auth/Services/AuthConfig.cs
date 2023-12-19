using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taogar.Finance.Auth.Services
{
    public class AuthConfig
    {
        private readonly IConfiguration _configuration;

        public AuthConfig()
        {
            IConfiguration configurationManager = new ConfigurationBuilder() //Читаем конфигурации 
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            _configuration = configurationManager;

            var configuration = configurationManager.GetSection("JwtSettings");
            AppName = configuration["AppName"];
            Certificate = configuration["Certificate"];
            Issuer = configuration["Issuer"];
            Audience = configuration["Audience"];

            var IDes = configuration.GetSection("IDes");

            AppId = IDes["AppKeyCloakId"];

            var roles = IDes.GetSection("Roles");
        }

        public string AppName { get; private set; }
        public string Certificate { get; private set; }
        public string Issuer { get; private set; }
        public string Audience { get; private set; }

        public string AppId { get; private set; }

        

        public string RoleId(string role)
        {
            return _configuration.GetSection("JwtSettings").GetSection("IDes").GetSection("Roles")[role];
        }
    }
}
