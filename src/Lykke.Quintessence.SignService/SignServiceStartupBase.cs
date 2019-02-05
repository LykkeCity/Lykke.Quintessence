using System;
using System.Reflection;
using FluentValidation.AspNetCore;
using Lykke.Quintessence.Modules;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;

namespace Lykke.Quintessence
{
    public abstract class SignServiceStartupBase<T> : ServiceStartupBase<AppSettings<T>>
        where T : SignServiceSettings
    {   
        protected override bool IsDebug
            => SignServiceStarter.IsDebug;

        protected override string ServiceName
            => "SignService";

        protected override void ConfigureFluentValidation(
            FluentValidationMvcConfiguration configuration)
        {
            base.ConfigureFluentValidation(configuration);
            
            configuration.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureLogging(LykkeLoggingOptions<AppSettings<T>> options)
        {
            ConfigureLogging(options, LoggingMode.Empty);
        }

        protected override void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            modules.RegisterModule<SignServiceModule<T>>();
            
            base.RegisterAdditionalModules(modules);
        }

        protected override string ResolveLogsConnString(
            AppSettings<T> settings)
        {
            throw new NotSupportedException("Logging is not supported for sign service.");
        }
    }
}