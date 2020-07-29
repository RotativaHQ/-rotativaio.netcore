using System;
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
}
