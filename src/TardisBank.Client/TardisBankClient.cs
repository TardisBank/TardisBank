using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TardisBank.Dto;

namespace TardisBank.Client
{
    public static class TardisBankClient
    {
        const string jsonMediaType = "application/json";

        public static Task<HomeUnauthenticatedResponse> GetHome(this ClientConfig config)
            => config.Get<HomeUnauthenticatedResponse>(new LinkModel 
            {
                Rel = "home",
                Href = "/"
            });

        public static Task<T> Get<T>(this ClientConfig config, LinkModel link)
            => Send<T>(config, link, HttpMethod.Get);

        public static Task<TResponse> Post<TRequest, TResponse>(this ClientConfig config, LinkModel link, TRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            var requestBody = JsonConvert.SerializeObject(request);
            return config.Send<TResponse>(link, HttpMethod.Post, requestBody);
        }

        public static async Task<T> Send<T>(
            this ClientConfig config, 
            LinkModel link, 
            HttpMethod method,
            string requestBody = null)
        {
            if(link == null) throw new ArgumentNullException(nameof(link));

            var requestUri = new UriBuilder(config.BaseUri)
            {
                Path = link.Href
            }.Uri;
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonMediaType));

            if(requestBody != null)
            {
                request.Content = new StringContent(requestBody, Encoding.UTF8, jsonMediaType);
            }

            if(config.HasToken)
            {
                request.Headers.Add("Authorization", $"Bearer {config.Token}");
            }

            var response = await config.HttpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }

    public class ClientConfig
    {
        public Uri BaseUri { get; }
        public HttpClient HttpClient { get; }
        public string Token { get; }

        public ClientConfig(Uri baseUri, HttpClient httpClient, string token = null)
        {
            BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Token = token;
        }

        public bool HasToken => Token != null;
    }
}