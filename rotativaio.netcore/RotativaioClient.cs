using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RotativaIO.NetCore
{
    public class RotativaioClient
    {
        string apiUrl;
        string apiKey;

        public RotativaioClient(string apiKey, string apiUrl)
        {
            this.apiKey = apiKey;
            this.apiUrl = apiUrl;
        }

        public async Task<string> GetPdfUrl(string switches, string html, string fileName = "", string header = "", string footer = "", string contentDisposition = "")
        {
            var payload = new
            {
                html = html,
                customSwitches = switches,
                returnLink = true
            };
            var httpClient = new HttpClient();
            using (
                var request = CreateRequest("/", "application/json", HttpMethod.Post))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var json = JsonConvert.SerializeObject(payload);
                    HttpContent content = new StringContent(json);
                    request.Content = content;

                    using (
                        HttpResponseMessage response =
                            await httpClient.SendAsync(request, new CancellationTokenSource().Token))
                    {
                        var httpResponseMessage = response;
                        var result = response.Content.ReadAsStringAsync();
                        var jsonReponse = JObject.Parse(result.Result);
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            var error = jsonReponse["error"].Value<string>();
                            throw new UnauthorizedAccessException(error);
                        }
                        var pdfUrl = jsonReponse["pdfUrl"].Value<string>(); // 
                        return pdfUrl;
                    }
                }
            }
        }

        /// <summary>
        /// This method is taken from Filip W in a blog post located at: http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mthv"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        protected HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = CreateRawRequest(url, mthv, method);
            request.Headers.Add("X-ApiKey", apiKey);
            return request;
        }

        protected HttpRequestMessage CreateRawRequest(string url, string mthv, HttpMethod method)
        {
            //if (!ConfigurationManager.AppSettings.AllKeys.Contains("RotativaUrl"))
            //{
            //    throw new Exception("RotativaUrl AppSetting not found");
            //}
            //var apiUrl = ConfigurationManager.AppSettings["RotativaUrl"].ToString();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(apiUrl + url)
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;
            //Debug.WriteLine("Method: " + request.Method);
            //Debug.WriteLine("URL: " + request.RequestUri);
            //Debug.WriteLine("Headers: ");
            //Debug.WriteLine("\t" + request.Headers.ToString());
            return request;
        }
    }
}
