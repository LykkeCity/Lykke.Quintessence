using Lykke.BilService.SampleApi.Modules;
using Lykke.Quintessence;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;

namespace Lykke.BilService.SampleApi
{
    public class Startup : ApiStartupBase<ApiSettings>
    {
        protected override string IntegrationName
            => "Sample";
        
        protected override void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            modules.RegisterModule<SampleApiModule>();
            
            base.RegisterAdditionalModules(modules);
        }
    }
}