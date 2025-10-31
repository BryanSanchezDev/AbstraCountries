using AbstraCountries.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AbstraCountries.Tests.Api.Controllers
{
    public abstract class ControllerTestBase
    {
        protected HttpClient? _client;
        private readonly AbstraCountriesApplicationFactory _factory = new();

        public virtual async Task BeforeEach()
        {
            _client = _factory.CreateClient();

            var token = await GetTokenAsync("admin", "password");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<string> GetTokenAsync(string username, string password)
        {
            var payload = JsonSerializer.Serialize(new { username, password });
            var resp = await _client!.PostAsync("/auth/login",
                new StringContent(payload, Encoding.UTF8, "application/json"));

            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("token").GetString()!;
        }
    }
}
