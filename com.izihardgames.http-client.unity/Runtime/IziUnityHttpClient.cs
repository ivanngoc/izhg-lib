#if !UNITY_WEBGL
using System.Collections;
using System.Net.Http;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace IziHardGames
{
    public class IziUnityHttpClient : IHttpClientSimple, IDisposable
    {
        private readonly HttpClient httpClient;

        public IziUnityHttpClient(HttpClientHandler handler)
        {
            httpClient = new HttpClient(handler);
        }

        public async ValueTask<T> RequestAsync<T>(string method, string uri, CancellationToken ct = default)
        {
            using (var request = new HttpRequestMessage(new HttpMethod(method), uri))
            {
                var response = await httpClient.SendAsync(request, ct).ConfigureAwait(false);
                return JsonUtility.FromJson<T>(await response.Content.ReadAsStringAsync());
            }
        }
        public async ValueTask<T> RequestAsync<T>(string method, string uri, string content, CancellationToken ct = default)
        {
            using (var request = new HttpRequestMessage(new HttpMethod(method), uri))
            {
                request.Content = new StringContent(content);
                var response = await httpClient.SendAsync(request, ct).ConfigureAwait(false);
                return JsonUtility.FromJson<T>(await response.Content.ReadAsStringAsync());
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}

#endif