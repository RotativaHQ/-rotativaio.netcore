using RazorEngineCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RotativaIO.NetCore
{
    public class PdfHelper: IDisposable
    {

        string apiUrl; 
        string apiKey;
        HttpClient httpClient;

        public PdfHelper(string apiKey, string apiUrl)
        {
            this.apiKey = apiKey;
            this.apiUrl = apiUrl;
            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// Returns properties with OptionFlag attribute as one line that can be passed to wkhtmltopdf binary.
        /// </summary>
        /// <returns>Command line parameter that can be directly passed to wkhtmltopdf binary.</returns>
        private string GetConvertOptions(RotativaOptions ro)
        {
            if (ro == null)
            {
                return string.Empty;
            }

            var result = new StringBuilder();

            if (ro.PageMargins != null)
                result.Append(ro.PageMargins.ToString());

            var fields = ro.GetType().GetProperties();
            foreach (var fi in fields)
            {
                var of = fi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() as OptionFlag;
                if (of == null)
                    continue;

                object value = fi.GetValue(ro, null);
                if (value == null)
                    continue;

                if (fi.PropertyType == typeof(Dictionary<string, string>))
                {
                    var dictionary = (Dictionary<string, string>)value;
                    foreach (var d in dictionary)
                    {
                        result.AppendFormat(" {0} {1} {2}", of.Name, d.Key, d.Value);
                    }
                }
                else if (fi.PropertyType == typeof(bool))
                {
                    if ((bool)value)
                        result.AppendFormat(CultureInfo.InvariantCulture, " {0}", of.Name);
                }
                else
                {
                    result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
                }
            }

            var switches = result.ToString().Trim();
            return switches;
        }


        internal string BuildHtml<T>(string templateText, T model)
        {
            RazorEngine razorEngine = new RazorEngine();

            // yeah, heavy definition
            var template = razorEngine.Compile<RazorEngineTemplateBase<T>>(templateText);

            string result = template.Run(instance =>
            {
                instance.Model = model;
            });

            return result;
        }

        public async Task<string> GetPdfUrl<T>(string templateText, T model, RotativaOptions rotativaOptions = null)
        {
            var rClient = new RotativaioClient(apiKey, apiUrl);
            var html = BuildHtml<T>(templateText, model);
            var switches = GetConvertOptions(rotativaOptions);
            var pdfUrl = await rClient.GetPdfUrl(switches, html);
            return pdfUrl;
        }    

        public async Task<Byte[]> GetPdfAsByteArray<T>(string templateText, T model, RotativaOptions rotativaOptions = null)
        {
            var pdfUrl = await GetPdfUrl<T>(templateText, model, rotativaOptions);
            var pdf = await httpClient.GetAsync(pdfUrl);
            var pdfBytes = await pdf.Content.ReadAsByteArrayAsync();
            return pdfBytes;
        }

        public async Task<Stream> GetPdfAsStream<T>(string templateText, T model, RotativaOptions rotativaOptions = null)
        {
            var pdfUrl = await GetPdfUrl<T>(templateText, model, rotativaOptions);
            var pdf = await httpClient.GetAsync(pdfUrl);
            var pdfStream = await pdf.Content.ReadAsStreamAsync();
            return pdfStream;
        }

        public void Dispose()
        {
            this.httpClient.Dispose();
        }
    }
}
