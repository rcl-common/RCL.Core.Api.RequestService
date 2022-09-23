# nullable disable

using RCL.Core.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RCL.Core.Api.RequestService
{
    public abstract class ApiRequestBase
    {
        private readonly IAuthTokenService _authTokenService;
        private static readonly HttpClient _client;

        private string _accessToken = string.Empty;

        static ApiRequestBase()
        {
            _client = new HttpClient();
        }

        public ApiRequestBase(IAuthTokenService authTokenService)
        {
            _authTokenService = authTokenService;
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string resource)
            where TResult : new()
        {
            try
            {
                await SetRequestHeadersAsync(resource);

                var response = await _client.GetAsync(uri);
                string content = ResolveContent(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                    TResult obj = JsonSerializer.Deserialize<TResult>(content);
                    return obj;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return new TResult();
                    }
                    else
                    {
                        throw new Exception($"{response.StatusCode} : {content}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TResult>> GetListResultAsync<TResult>(string uri, string resource)
            where TResult : class
        {
            try
            {
                await SetRequestHeadersAsync(resource);

                var response = await _client.GetAsync(uri);
                string content = ResolveContent(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                    List<TResult> list = JsonSerializer.Deserialize<List<TResult>>(content);
                    return list;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return new List<TResult>();
                    }
                    else
                    {
                        throw new Exception($"{response.StatusCode} : {content}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task PostAsync<T>(string uri, string resource, T payload)
            where T : class
        {
            try
            {
                await SetRequestHeadersAsync(resource);

                var response = await _client.PostAsync(uri,
                     new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                string content = ResolveContent(await response.Content.ReadAsStringAsync());

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{response.StatusCode} : {content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TResult> PostAsync<T, TResult>(string uri, string resource, T payload)
            where TResult : new()
            where T : class
        {
            try
            {
                await SetRequestHeadersAsync(resource);

                var response = await _client.PostAsync(uri,
                     new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                string content = ResolveContent(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                    TResult obj = JsonSerializer.Deserialize<TResult>(content);
                    return obj;
                }
                else
                {
                    throw new Exception($"{response.StatusCode} : {content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TResult> PutAsync<T, TResult>(string uri, string resource, T payload)
           where TResult : new()
           where T : class
        {
            try
            {
                await SetRequestHeadersAsync(resource);

                var response = await _client.PutAsync(uri,
                     new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                string content = ResolveContent(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                    TResult obj = JsonSerializer.Deserialize<TResult>(content);
                    return obj;
                }
                else
                {
                    throw new Exception($"{response.StatusCode} : {content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TResult>> PostListResultAsync<T, TResult>(string uri, string resource, T payload)
          where TResult : class
          where T : class
        {
            try
            {
                await SetRequestHeadersAsync(resource);

                var response = await _client.PostAsync(uri,
                     new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                string content = ResolveContent(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                    List<TResult> list = JsonSerializer.Deserialize<List<TResult>>(content);
                    return list;
                }
                else
                {
                    throw new Exception($"{response.StatusCode} : {content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(string uri, string resource)
        {
            try
            {
                await SetRequestHeadersAsync(resource);

                var response = await _client.DeleteAsync(uri);

                string content = ResolveContent(await response.Content.ReadAsStringAsync());

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{response.StatusCode} : {content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SetRequestHeadersAsync(string resource)
        {
            string accessToken = await GetAccessTokenAsync(resource);

            _client.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(accessToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        private async Task<string> GetAccessTokenAsync(string resource)
        {
            try
            {
                if (_accessToken == string.Empty)
                {
                    AuthToken authToken = await _authTokenService.GetAuthTokenAsync(resource);
                    _accessToken = authToken.access_token;
                }

                return _accessToken;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string ResolveContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            else
            {
                if (content.ToLower().Contains("!doctype html"))
                {
                    return string.Empty;
                }
                else
                {
                    return content;
                }
            }
        }
    }
}
