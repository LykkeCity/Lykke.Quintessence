using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Repositories.DependencyInjection;
using Lykke.Quintessence.Domain.Services.DependencyInjection;
using Lykke.Quintessence.RpcClient.DependencyInjection;
using Lykke.Quintessence.Settings;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Modules
{
    [UsedImplicitly]
    public class ApiModule<T> : Module
        where T : ApiSettings
    {
        // ReSharper disable once NotAccessedField.Local : Intended for future use
        private readonly IReloadingManager<AppSettings<T>> _appSettings;
        
        public ApiModule(
            IReloadingManager<AppSettings<T>> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            var apiSettings = _appSettings.Nested(x => x.Api);
            var connectionString = _appSettings.ConnectionString(x => x.Api.Db.DataConnString);
            var rpcNodeSettings = apiSettings.CurrentValue.RpcNode;

            builder
                .UseRpcClient
                (
                    rpcNodeSettings.ApiUrl,
                    rpcNodeSettings.ConnectionTimeout,
                    rpcNodeSettings.EnableTelemetry
                )
                .AddDefaultApi()
                .AddDefaultParityApi();

            builder
                .UseAzureRepositories(connectionString)
                .AddDefaultBalanceRepository()
                .AddDefaultBalanceMonitoringTaskRepository()
                .AddDefaultBlacklistedAddressRepository()
                .AddDefaultTransactionMonitoringTaskRepository()
                .AddDefaultTransactionReceiptRepository()
                .AddDefaultTransactionRepository()
                .AddDefaultWhitelistedAddressRepository();

            builder
                .RegisterServices
                (
                    apiSettings.Nested(x => x.ConfirmationLevel),
                    apiSettings.Nested(x => x.GasPriceRange),
                    apiSettings.Nested(x => x.GasReserve),
                    apiSettings.Nested(x => x.MaxGasAmount)
                )
                .AddDefaultAddressService()
                .AddDefaultBalanceService()
                .AddDefaultTransactionHistoryService()
                .AddDefaultTransactionService();
        }
    }
}