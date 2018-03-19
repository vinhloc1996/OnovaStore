using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace OnovaStore.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static dynamic JsonDataFromApi(string url, string method, StringContent data = null)
        {
            dynamic result = null;
            if (!string.IsNullOrEmpty(url))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5000");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    switch (method)
                    {
                        case "post":
                            result = client.PostAsync(url, data).Result;
                            break;
                        case "get":
                            result = client.GetAsync(url).Result;
                            break;
                    }
                }
            }

            return result;
        }
    }
}