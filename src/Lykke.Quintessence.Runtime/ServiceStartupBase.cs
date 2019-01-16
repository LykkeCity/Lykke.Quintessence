using System;
using FluentValidation.AspNetCore;
using JetBrains.Annotations;
using Lykke.Logs;
using Lykke.Logs.Loggers.LykkeSlack;
using Lykke.Sdk;
using Lykke.Sdk.Settings;
using Lykke.SettingsReader;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;


namespace Lykke.Quintessence
{
    [PublicAPI]
    public abstract class ServiceStartupBase<T>
        where T : BaseAppSettings 
    {
        protected abstract string IntegrationName { get; }
        
        protected abstract bool IsDebug { get; }
        
        protected abstract string ServiceName { get; }
        
        public void Configure(
            IApplicationBuilder app)
        {
            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = GetSwaggerOptions();

                ConfigureOptions(options);
            });
        }

        public IServiceProvider ConfigureServices(
            IServiceCollection services)
        {
            return services.BuildServiceProvider<T>(options =>
            {
                options.ConfigureFluentValidation = ConfigureFluentValidation;
                options.Extend = ConfigureServices;
                options.Logs = ConfigureLogging;
                options.RegisterAdditionalModules = RegisterAdditionalModules;
                options.Swagger = ConfigureSwagger;
                options.SwaggerOptions = GetSwaggerOptions();
            });
        }
        
        protected virtual void ConfigureFluentValidation(
            FluentValidationMvcConfiguration configuration)
        {
            
        }

        protected virtual void ConfigureDebugLogging(
            LykkeLoggingOptions<T> options)
        {
            options.AzureTableName = GetLogsTableName();
            options.AzureTableConnectionStringResolver = ResolveLogsConnString;
            
            options.Extended = extendedLogs =>
            {
                extendedLogs.SetMinimumLevel(LogLevel.Debug);
            };
        }
        
        protected void ConfigureEmptyLogging(
            LykkeLoggingOptions<T> options)
        {
            options.UseEmptyLogging();
        }

        protected virtual void ConfigureLogging(
            LykkeLoggingOptions<T> options)
        {
            ConfigureLogging
            (
                options,
                IsDebug ? LoggingMode.Debug : LoggingMode.Release
            );
        }
        
        protected void ConfigureLogging(
            LykkeLoggingOptions<T> options,
            LoggingMode mode)
        {
            switch (mode)
            {
                case LoggingMode.Empty:
                    ConfigureEmptyLogging(options);
                    break;
                case LoggingMode.Debug:
                    ConfigureDebugLogging(options);
                    break;
                case LoggingMode.Release:
                    ConfigureReleaseLogging(options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        
        protected virtual void ConfigureReleaseLogging(
            LykkeLoggingOptions<T> options)
        {
            options.AzureTableName = GetLogsTableName();
            options.AzureTableConnectionStringResolver = ResolveLogsConnString;
            
            options.Extended = extendedLogs =>
            {
                extendedLogs.AddAdditionalSlackChannel("BlockChainIntegration", channelOptions =>
                {
                    channelOptions.MinLogLevel = LogLevel.Information;
                    channelOptions.SpamGuard.DisableGuarding();
                    channelOptions.IncludeHealthNotifications();
                });
                        
                extendedLogs.AddAdditionalSlackChannel("BlockChainIntegrationImportantMessages", channelOptions =>
                {
                    channelOptions.MinLogLevel = LogLevel.Warning;
                    channelOptions.SpamGuard.DisableGuarding();
                    channelOptions.IncludeHealthNotifications();
                });
            };
        }
        
        protected virtual void ConfigureOptions(
            LykkeConfigurationOptions options)
        {
            
        }
        
        protected virtual void ConfigureServices(
            IServiceCollection services,
            IReloadingManager<T> settings)
        {
            
        }

        protected virtual void ConfigureSwagger(
            SwaggerGenOptions options)
        {
            
        }

        protected virtual string GetLogsTableName()
        {
            return $"{IntegrationName}{ServiceName}Log";
        }
        
        protected virtual LykkeSwaggerOptions GetSwaggerOptions()
        {
            return new LykkeSwaggerOptions
            {
                ApiTitle = $"{IntegrationName} {ServiceName}",
                ApiVersion = "v1"
            };
        }

        protected virtual void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            
        }

        protected abstract string ResolveLogsConnString(
            T settings);
    }
}