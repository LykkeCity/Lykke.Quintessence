using Lykke.BilService.SampleSignService.Modules;
using Lykke.Quintessence;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;

namespace Lykke.BilService.SampleSignService
{
    public class Startup : SignServiceStartupBase<SignServiceSettings>
    {
        protected override string IntegrationName
            => "Sample";

        protected override void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            modules.RegisterModule<SampleSignServiceModule>();
            
            base.RegisterAdditionalModules(modules);
        }
    }
}