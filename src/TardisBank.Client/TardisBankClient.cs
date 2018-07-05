using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TardisBank.Dto;

namespace TardisBank.Client
{
    public class TardisBankClient
    {
        public Uri BaseUri { get; }
        public HttpClient HttpClient { get; }

        const string jsonMediaType = "application/json";

        public TardisBankClient(Uri baseUri, HttpClient httpClient)
        {
            BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<HomeUnauthenticatedResponse> GetHome()
            => Get<HomeUnauthenticatedResponse>(new LinkModel 
            {
                Rel = "home",
                Href = "/"
            });

        public Task<T> Get<T>(LinkModel link)
            => Send<T>(link, HttpMethod.Get);

        public Task<TResponse> Post<TRequest, TResponse>(LinkModel link, TRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            var requestBody = JsonConvert.SerializeObject(request);
            return Send<TResponse>(link, HttpMethod.Post, requestBody);
        }

        public async Task<T> Send<T>(
            LinkModel link, 
            HttpMethod method,
            string requestBody = null)
        {
            if(link == null) throw new ArgumentNullException(nameof(link));

            var requestUri = new UriBuilder(BaseUri)
            {
                Path = link.Href
            }.Uri;
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonMediaType));

            if(requestBody != null)
            {
                request.Content = new StringContent(requestBody, Encoding.UTF8, jsonMediaType);
            }

            var response = await HttpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}