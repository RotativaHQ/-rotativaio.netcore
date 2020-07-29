using System;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace RotativaIO.NetCore.demoAzureFunction
{
    public class TestModel
    {
        public string Name { get; set; }
    }

    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run(
            [TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer,
            [SendGrid(ApiKey = "SendGridApiKey")] IAsyncCollector<SendGridMessage> messageCollector,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var rotativaioKey = config["RotativaIOKey"];

            var template = "Hello @Model.Name";
            var model = new TestModel { Name = "Giorgio" };
            using (var pdfHelper = new PdfHelper(rotativaioKey, "https://eunorth.rotativahq.com"))
            { 

                var pdfBytes = await pdfHelper.GetPdfAsByteArray(template, model, new RotativaOptions { PageSize = Size.A5 });

                var message = new SendGridMessage();
                message.AddTo("giorgio.bozio@gmail.com");
                message.AddContent("text/plain", $"Invio file pdf di test");
                var attachmentContent = Convert.ToBase64String(pdfBytes);
                var attach = new Attachment();
                message.AddAttachment("fatture_rotativa.pdf", attachmentContent, "application/pdf");
                message.SetFrom(new EmailAddress("giorgio.bozio@gmail.com"));
                message.SetSubject("Test pdf netcore");

                await messageCollector.AddAsync(message);
            }

        }
        public static string ConvertToBase64(Stream stream)
        {
            MemoryStream ms = new MemoryStream();

            stream.CopyTo(ms);
            var bytes = ms.ToArray();
            var base64string = Convert.ToBase64String(bytes);
            return base64string;
        }
    }
}
