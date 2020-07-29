using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;

namespace RotativaIO.NetCore.Azure
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public sealed class RotativaIOAttribute : Attribute, IExtensionConfigProvider
    {
        [AppSetting]
        public string ApiKeySetting { get; set; }


        [AppSetting]
        public string EndpointUrlSetting { get; set; }

        public void Initialize(ExtensionConfigContext context)
        {

            //context.AddConverter<PdfHelper, string>(ConvertToString);

            //// Create an input rules for the Sample attribute.
            //var rule = context.AddBindingRule<SampleAttribute>();

            //rule.BindToInput<SampleItem>(BuildItemFromAttr);
        }
    }
}
