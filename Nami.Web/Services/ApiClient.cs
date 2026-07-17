using System.Net.Http.Headers;
using System.Net.Http.Json;
using Nami.Web.State;

namespace Nami.Web.Services
{
    // Envuelve HttpClient hacia Nami.API: adjunta el token JWT y traduce errores.
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AuthState _authState;

        public ApiClient(HttpClient httpClient, AuthState authState)
        {
            _httpClient = httpClient;
            _authState = authState;
        }

        private void ApplyAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = _authState.Token is null
                ? null
                : new AuthenticationHeaderValue("Bearer", _authState.Token);
        }

        private static async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            string message = $"Error de comunicación con la API ({(int)response.StatusCode}).";
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ErrorMessage>();
                if (!string.IsNullOrWhiteSpace(problem?.Message))
                    message = problem.Message;
            }
            catch
            {
                // El cuerpo no era JSON con "message"; se conserva el mensaje genérico.
            }

            throw new ApiException(message);
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            ApplyAuthHeader();
            var response = await _httpClient.GetAsync(url);
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse?> PostAsync<TResponse>(string url, object? body = null)
        {
            ApplyAuthHeader();
            var response = await _httpClient.PostAsJsonAsync(url, body);
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PostAsync(string url, object? body = null)
        {
            ApplyAuthHeader();
            var response = await _httpClient.PostAsJsonAsync(url, body);
            await EnsureSuccess(response);
        }

        public async Task<TResponse?> PutAsync<TResponse>(string url, object body)
        {
            ApplyAuthHeader();
            var response = await _httpClient.PutAsJsonAsync(url, body);
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse?> PatchAsync<TResponse>(string url, object body)
        {
            ApplyAuthHeader();
            var response = await _httpClient.PatchAsJsonAsync(url, body);
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PatchAsync(string url, object body)
        {
            ApplyAuthHeader();
            var response = await _httpClient.PatchAsJsonAsync(url, body);
            await EnsureSuccess(response);
        }

        public async Task DeleteAsync(string url)
        {
            ApplyAuthHeader();
            var response = await _httpClient.DeleteAsync(url);
            await EnsureSuccess(response);
        }

        public async Task<TResponse?> DeleteAsync<TResponse>(string url)
        {
            ApplyAuthHeader();
            var response = await _httpClient.DeleteAsync(url);
            await EnsureSuccess(response);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        private record ErrorMessage(string? Message);
    }
}
