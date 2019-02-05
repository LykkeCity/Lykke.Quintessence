using System.Reflection;
using FluentValidation.AspNetCore;
using Lykke.Quintessence.Modules;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Quintessence
{
    public abstract class ApiStartupBase<T> : ServiceStartupBase<AppSettings<T>>
        where T : ApiSettings
    {   
        protected override bool IsDebug
            => ApiStarter.IsDebug;

        protected override string ServiceName
            => "Api";

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
            
            modules.RegisterModule<ApiModule<T>>();
        }

        protected override string ResolveLogsConnString(
            AppSettings<T> settings)
        {
            return settings.Api.Db.LogsConnString;
        }
    }
}