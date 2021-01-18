using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RotativaIO.NetCore.Tests
{
    [Trait("RotativaioClient", "calling the service")]
    public class RotativaClientTests
    {
        [Fact(DisplayName = "should return a valid pdf url")]
        public async Task OK()
        {
            var cli = new RotativaioClient("3b8626bf9ad74c98b7f641a8e668e1db", "https://eunorth.rotativahq.com");
            var res = await cli.GetPdfUrl("", "<b>Ciao</b>", "", "", "", "");
            Assert.NotEmpty(res);
        }
    }

    [Trait("Rotativaio PdfHelper", "getting PDF bytes array")]
    public class RotativaPdfHelperTests
    {
        [Fact(DisplayName = "should return a valid pdf")]
        public async Task OK()
        {
            var template = "Hello @Model.Name";
            var model = new TestModel { Name = "Giorgio" };
            using (var pdfHelper = new PdfHelper("3b8626bf9ad74c98b7f641a8e668e1db", "https://eunorth.rotativahq.com"))
            {
                foreach (var index in Enumerable.Range(1, 1))
                {
                    var pdfBytes = await pdfHelper.GetPdfAsByteArray<TestModel>(template, model, new RotativaOptions { PageSize = Size.A5 });
                    File.WriteAllBytes($"c:\\temp\\test{index}.pdf", pdfBytes);
                }
            }
        }
    }

    // You can define other methods, fields, classes and namespaces here
    public class TestModel
    {
        public string Name { get; set; }
    }
}
