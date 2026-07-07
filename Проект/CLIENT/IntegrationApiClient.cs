using System.Net.Http;
using System.Text;
using System.Text.Json;
using WmsClient.Models;

namespace WmsClient.Services
{
    // Единственный класс, который общается с сервером API.
    // Сама база данных здесь не используется — только сетевой вызов.
    public class IntegrationApiClient
    {
        private static readonly HttpClient _client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

        public async Task<(bool Success, string Message)> SendProductsToOneCAsync(List<Product> products)
        {
            try
            {
                var url = $"{AppSettings.ApiBaseUrl}/api/v1/send-to-1c";

                var json = JsonSerializer.Serialize(products);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(url, content);
                var body = await response.Content.ReadAsStringAsync();

                return (response.IsSuccessStatusCode, body);
            }
            catch (Exception ex)
            {
                return (false, $"Не удалось подключиться к API ({AppSettings.ApiBaseUrl}): {ex.Message}");
            }
        }
    }
}
