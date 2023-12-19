using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Taogar.Finance.Auth.Interfaces;

namespace Taogar.Finance.Auth.Services
{
    internal class KeyCloakService : IKeyCloakService
    {
        private readonly HttpClient httpClient;
        private string Token = string.Empty;
        private string RefrashToken = string.Empty;
        private readonly AuthConfig config;

        public KeyCloakService(AuthConfig authConfig)
        {
            config = authConfig;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8080");
        }

        private async Task<string> Autorize()
        {
            var requestData = new Dictionary<string, string>
            {
                { "client_id", "admin-cli" },
                { "grant_type", "password" },
                { "username", "Taogar" },
                { "password", "Sq*fG40Kru7" }
            };
            var requestContent = new FormUrlEncodedContent(requestData);

            var response = httpClient.PostAsync("/realms/master/protocol/openid-connect/token", requestContent).Result;

            if (response.IsSuccessStatusCode)
            {
                // Читаем содержимое ответа (в формате JSON)
                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                return jsonObject.access_token;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> GetIdByParams(string firstName, string lastName, string email)
        {
            Token = await Autorize();
            string path = $"/admin/realms/TaogarSmartCloud/users?email={email}&firstName={firstName}&lastName={lastName}";
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            var response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                // Читаем содержимое ответа (в формате JSON)
                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                return jsonObject[0].id;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<HttpStatusCode> AssingRoleToUser(string firstName, string lastName, string email, string roleName)
        {
            Token = await Autorize();
            string UserId = await GetIdByParams(firstName, lastName, email);

            string path = $"/admin/realms/TaogarSmartCloud/users/{UserId}/role-mappings/clients/{config.AppId}";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>()
            {
                new { id = $"{config.RoleId(roleName)}", name = roleName }
            });

            StringContent content = new StringContent(json, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
            var response = await httpClient.PostAsync(path, content);
            return response.EnsureSuccessStatusCode().StatusCode;
        }
    }
}
