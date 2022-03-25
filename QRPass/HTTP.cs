
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace QRPass
{
    class HTTP
    {
        string BaseAddress = "https://qrpasscon.ru/";
        string requsetUri;
        string name;
        string value;

        public HTTP setHeaders(string name, string value)
        {
            this.value = value;
            this.name = name;
            return this;
        }

        IEnumerable<KeyValuePair<string, string>> bodyRequest;

        public HTTP setBodyRequest(IEnumerable<KeyValuePair<string, string>> bodyRequest)
        {
            this.bodyRequest = bodyRequest;
            return this;
        }

        public HTTP setRequestUri(string requsetUrl)
        {
            requsetUri = requsetUrl;
            return this;
        }



        public string get() {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                HTTP http = new HTTP();
                HttpResponseMessage response = client.GetAsync(requsetUri).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string post() {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                FormUrlEncodedContent content = new FormUrlEncodedContent(bodyRequest);
                client.DefaultRequestHeaders.TryAddWithoutValidation(name, value);
                HttpResponseMessage result = client.PostAsync(requsetUri, content).Result;
                return result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
