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
                { "username", config.KeyCloakAdminUserName },
                { "password", config.KeyCloakAdminUserPassword }
            };
            var requestContent = new FormUrlEncodedContent(requestData);

            var response = httpClient.PostAsync("/realms/master/protocol/openid-connect/token", requestContent).Result;

            if (response.IsSuccessStatusCode)
            {
                // Читаем содержимое ответа (в формате JSON)
                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

                if (httpClient.DefaultRequestHeaders.FirstOrDefault(x => x.Key == "Authorization").Value == null)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jsonObject.access_token}");
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jsonObject.access_token}");
                }
                return jsonObject.access_token;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> GetIdByParams(string firstName, string lastName, string email)
        {
            await Autorize();
            string path = $"/admin/realms/TaogarSmartCloud/users?email={email}&firstName={firstName}&lastName={lastName}";
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
            Autorize();
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

        public async Task<HttpStatusCode> CreateKeyCloakUser(string _firstName, string _lastName, string _userName, string _email)
        {
            await Autorize();

            string path = $"/admin/realms/TaogarSmartCloud/users";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                email = _email,
                firstName = _firstName,
                lastName = _lastName,
                enabled = true,
                username = _userName,
                emailVerified = true,
                credentials = new List<object>
                {
                    new 
                    { 
                        type = "password", 
                        value =  "0000",
                        temporary = true
                    }
                }
            });

            StringContent content = new StringContent(json, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
            var response = await httpClient.PostAsync(path, content);
            return response.StatusCode;
        }
    }
}
