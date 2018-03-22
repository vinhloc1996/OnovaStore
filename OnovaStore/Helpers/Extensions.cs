using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public static async Task<dynamic> JsonDataFromApi(string url, string method, StringContent data = null)
        {
            dynamic result = null;
            if (!string.IsNullOrEmpty(url))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5000");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = null;
                    switch (method)
                    {
                        case "post":
                            response = await client.PostAsync(url, data);
                            if (response.IsSuccessStatusCode)
                            {
                                result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                            }
                            break;
                        case "get":
                            response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode)
                            {
                                result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                            }
                            break;
                    }
                }
            }

            return result;
        }

        public static Int64 ToUnixEpochDate(this DateTime dateTime)
        {
            var result = (Int64)Math.Round((dateTime.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

            return result;
        }

        public static DateTime ToUnixEpochDate(this Int64 unixTime)
        {
            var result = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);

            return result;
        }
    }

    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}