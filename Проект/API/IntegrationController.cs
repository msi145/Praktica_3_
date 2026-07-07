using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WmsApi.Models;

namespace WmsApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class IntegrationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _config;

        public IntegrationController(IHttpClientFactory httpFactory, IConfiguration config)
        {
            _httpFactory = httpFactory;
            _config = config;
        }

        // Проверка того, что сервер жив: GET https://localhost:7192/api/v1/health
        [HttpGet("health")]
        public IActionResult Health() => Ok(new { status = "ok", time = DateTime.Now });

        // Приём данных из WPF и пересылка в веб-сервис 1С:
        // POST https://localhost:7192/api/v1/send-to-1c
        [HttpPost("send-to-1c")]
        public async Task<IActionResult> SendToOneC([FromBody] List<ProductDto> products)
        {
            if (products is null || products.Count == 0)
                return BadRequest(new { message = "Список товаров пуст — нечего передавать." });

            var url = _config["OneC:ServiceUrl"];
            if (string.IsNullOrWhiteSpace(url))
                return StatusCode(500, new { message = "В appsettings.json не задан OneC:ServiceUrl." });

            var client = _httpFactory.CreateClient();

            try
            {
                var json = JsonSerializer.Serialize(products);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var login = _config["OneC:Login"];
                var password = _config["OneC:Password"];
                if (!string.IsNullOrEmpty(login))
                {
                    var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{login}:{password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
                }

                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new
                    {
                        success = true,
                        message = $"Передано в 1С позиций: {products.Count}",
                        oneCResponse = responseBody
                    });
                }

                return StatusCode((int)response.StatusCode, new
                {
                    success = false,
                    message = "1С отклонила запрос.",
                    details = responseBody
                });
            }
            catch (Exception ex)
            {
                // Типичная ситуация в разработке: HTTP-сервис 1С ещё не опубликован или недоступен.
                return StatusCode(502, new
                {
                    success = false,
                    message = "Не удалось связаться с сервисом 1С. Проверьте, что база 1С опубликована и что OneC:ServiceUrl в appsettings.json указан верно.",
                    error = ex.Message
                });
            }
        }
    }
}
