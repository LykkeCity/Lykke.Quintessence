using System.Reflection;
using FluentValidation.AspNetCore;
using Lykke.Quintessence.Modules;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Quintessence
{
    public abstract class JobStartupBase<T> : ServiceStartupBase<AppSettings<T>>
        where T : JobSettings
    {
        protected override bool IsDebug
            => JobStarter.IsDebug;

        protected override string ServiceName
            => "Job";

        protected override void ConfigureFluentValidation(
            FluentValidationMvcConfiguration configuration)
        {
            base.ConfigureFluentValidation(configuration);
            
            configuration.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
        protected override void ConfigureServices(
            IServiceCollection services,
            IReloadingManager<AppSettings<T>> settings)
        {
            base.ConfigureServices(services, settings);

            services
                .AddHttpClient();
        }

        protected override void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            base.RegisterAdditionalModules(modules);
            
            modules.RegisterModule<JobModule<T>>();
        }

        protected override string ResolveLogsConnString(
            AppSettings<T> settings)
        {
            return settings.Job.Db.LogsConnString;
        }
    }
}