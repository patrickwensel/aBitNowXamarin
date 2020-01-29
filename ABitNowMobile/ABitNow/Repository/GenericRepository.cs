using ABitNow.Constants;
using ABitNow.Contracts.Repository;
using ABitNow.Exceptions;
using Newtonsoft.Json;
using Polly;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ABitNow.Repository
{
    public class GenericRepository : IGenericRepository
    {
        public async Task<T> GetAsync<T>(string uri, string authToken = "")
        {
            string jsonResult = string.Empty;
            HttpResponseMessage responseMessage = null;
            try
            {
                HttpClient httpClient = CreateHttpClient(authToken);

                responseMessage = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                        return true;
                    })
                    .WaitAndRetryAsync
                    (
                        3,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await httpClient.GetAsync(uri));

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(jsonResult))
                {
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }
                else
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ ex.GetType().Name + " : " + ex.Message}");
                if (responseMessage != null)
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult, ex);
                }
                else
                {
                    throw new HttpRequestExceptionEx(HttpStatusCode.NotAcceptable, jsonResult, ex);
                }
            }
        }

        public async Task<T> PostAsync<T>(string uri, T data, string authToken = "")
        {
            string jsonResult = string.Empty;
            HttpResponseMessage responseMessage = null;
            try
            {
                HttpClient httpClient = CreateHttpClient(authToken);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseMessage = await Policy
                   .Handle<WebException>(ex =>
                   {
                       Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                       return true;
                   })
                   .WaitAndRetryAsync
                   (
                       3,
                       retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                   )
                   .ExecuteAsync(async () => await httpClient.PostAsync(uri, content));

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(jsonResult))
                {
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }
                else
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ ex.GetType().Name + " : " + ex.Message}");
                if (responseMessage != null)
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult, ex);
                }
                else
                {
                    throw new HttpRequestExceptionEx(HttpStatusCode.NotAcceptable, jsonResult, ex);
                }
            }
        }

        public async Task<TR> PostAsync<T, TR>(string uri, T data, string authToken = "")
        {
            string jsonResult = string.Empty;
            HttpResponseMessage responseMessage = null;

            try
            {
                HttpClient httpClient = CreateHttpClient(authToken);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseMessage = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                        return true;
                    })
                    .WaitAndRetryAsync
                    (
                        3,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await httpClient.PostAsync(uri, content));

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(jsonResult))
                {
                    var json = JsonConvert.DeserializeObject<TR>(jsonResult);
                    return json;
                }
                else
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ ex.GetType().Name + " : " + ex.Message}");
                if (responseMessage != null)
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult, ex);
                }
                else
                {
                    throw new HttpRequestExceptionEx(HttpStatusCode.NotAcceptable, jsonResult, ex);
                }
            }
        }

        public async Task<T> PutAsync<T>(string uri, T data, string authToken = "")
        {
            string jsonResult = string.Empty;
            HttpResponseMessage responseMessage = null;
            try
            {
                HttpClient httpClient = CreateHttpClient(authToken);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseMessage = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                        return true;
                    })
                    .WaitAndRetryAsync
                    (
                        3,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await httpClient.PutAsync(uri, content));

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(jsonResult))
                {
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }
                else
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ ex.GetType().Name + " : " + ex.Message}");
                if (responseMessage != null)
                {
                    throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult, ex);
                }
                else
                {
                    throw new HttpRequestExceptionEx(HttpStatusCode.NotAcceptable, jsonResult, ex);
                }
            }
        }

        public async Task DeleteAsync(string uri, string authToken = "")
        {
            HttpClient httpClient = CreateHttpClient(authToken);
            await httpClient.DeleteAsync(uri);
        }

        private HttpClient CreateHttpClient(string authToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(ApiConstants.SubscriptionKey))
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiConstants.SubscriptionKey);
            }

            if (!string.IsNullOrEmpty(authToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }
            //if (!string.IsNullOrEmpty(token))
            //{
            //    client.DefaultRequestHeaders.Add("Authorization", token);
            //}
            return httpClient;
        }
    }
}
