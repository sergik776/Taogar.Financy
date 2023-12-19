using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taogar.Tests.Financy
{
    [TestFixture]
    public class RestApiTest
    {
        public static string Token { get; set; }

        private const string BaseUrl = "https://localhost:7082"; // Замените на ваш URL API
        private readonly HttpClient _httpClient;

        // Параметризованный тест с использованием атрибута TestCase
        [TestCase("/person")]
        [TestCase("/person/2")]
        public async Task PersonGet(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}{endpoint}");
            Assert.That(response.IsSuccessStatusCode, $"Failed with status code: {response.StatusCode}");
        }

        [TestCase("/person")]
        [TestCase("/person/2")]
        public async Task PersonPost(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}{endpoint}", null);
            Assert.That(response.IsSuccessStatusCode, $"Failed with status code: {response.StatusCode}");
        }

        [TestCase("/person")]
        [TestCase("/person/2")]
        public async Task PersonPut(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"{BaseUrl}{endpoint}", null);
            Assert.That(response.IsSuccessStatusCode, $"Failed with status code: {response.StatusCode}");
        }

        [TestCase("/person")]
        [TestCase("/person/2")]
        public async Task PersonDelete(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BaseUrl}{endpoint}");
            Assert.That(response.IsSuccessStatusCode, $"Failed with status code: {response.StatusCode}");
        }

        public RestApiTest() 
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {TokenMethod()}");
        }

        static string TokenMethod()
        {
            // Замените URL на фактический адрес вашего сервера Keycloak
            string tokenEndpoint = "http://localhost:8080/realms/TaogarSmartCloud/protocol/openid-connect/token";

            // Замените на ваши реальные учетные данные
            string clientId = "ScooterAuth";
            string grantType = "password";
            string username = "financy_manager_2";
            string password = "123456";

            using (HttpClient client = new HttpClient())
            {
                // Создаем данные для отправки в формате application/x-www-form-urlencoded
                var requestData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("grant_type", grantType),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

                // Отправляем POST-запрос
                HttpResponseMessage response = client.PostAsync(tokenEndpoint, requestData).Result;

                // Проверяем успешность запроса
                if (response.IsSuccessStatusCode)
                {
                    // Читаем содержимое ответа (в формате JSON)
                    string responseContent = response.Content.ReadAsStringAsync().Result;

                    var q = JsonConvert.DeserializeObject(responseContent);

                    return ((dynamic)q).access_token;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
