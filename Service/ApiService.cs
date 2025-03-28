using PokemonWinformsConsumir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonWinformsConsumir.Service
{

    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string _token = string.Empty;


        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7208/");
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginData = new LoginRequest { Username = username, Password = password };
            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var auth = JsonSerializer.Deserialize<AuthResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _token = auth?.Token ?? string.Empty;

                return !string.IsNullOrEmpty(_token);
            }

            return false;
        }

        public async Task<List<Pokemon>> GetPokemonsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.GetAsync("api/pokemons");

            if (!response.IsSuccessStatusCode)
                return new List<Pokemon>();

            var content = await response.Content.ReadAsStringAsync();
            var pokemons = JsonSerializer.Deserialize<List<Pokemon>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return pokemons ?? new List<Pokemon>();
        }

    }
}
