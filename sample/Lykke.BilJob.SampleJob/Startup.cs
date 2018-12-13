using Lykke.BilJob.SampleJob.Modules;
using Lykke.Quintessence;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;

namespace Lykke.BilJob.SampleJob
{
    public class Startup : JobStartupBase<JobSettings>
    {
        protected override string IntegrationName
            => "Sample";
        
        protected override void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            modules.RegisterModule<SampleJobModule>();
            
            base.RegisterAdditionalModules(modules);
        }
    }
}