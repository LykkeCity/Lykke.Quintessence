using System.Numerics;
using Autofac;
using Common;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Quintessence.DependencyInjection;
using Lykke.Quintessence.Domain.PeriodicalHandlers;
using Lykke.Quintessence.Domain.Repositories.DependencyInjection;
using Lykke.Quintessence.Domain.Services.DependencyInjection;
using Lykke.Quintessence.PeriodicalHandlers;
using Lykke.Quintessence.RpcClient.DependencyInjection;
using Lykke.Quintessence.Settings;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Modules
{
    [UsedImplicitly]
    public class JobModule<T> : Module
        where T : JobSettings
    {
        private readonly IReloadingManager<AppSettings<T>> _appSettings;
        
        public JobModule(
            IReloadingManager<AppSettings<T>> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            var connectionString = _appSettings.ConnectionString(x => x.Job.Db.DataConnString);
            var jobSettings = _appSettings.Nested(x => x.Job);
            var rpcNodeSettings = jobSettings.CurrentValue.RpcNode;

            BigInteger? maximalBalanceCheckPeriod = null;
            
            if (!string.IsNullOrEmpty(jobSettings.CurrentValue.MaximalBalanceCheckPeriod))
            {
                maximalBalanceCheckPeriod = BigInteger.Parse(jobSettings.CurrentValue.MaximalBalanceCheckPeriod);
            }
            
            
            builder
                .RegisterChaosKitty(_appSettings.CurrentValue.Chaos);
            
            builder
                .UseRpcClient
                (
                    rpcNodeSettings.ApiUrl,
                    rpcNodeSettings.ConnectionTimeout,
                    rpcNodeSettings.EnableTelemetry
                )
                .AddDefaultEthApi()
                .AddDefaultParityApi();

            builder
                .UseAzureRepositories(connectionString)
                .AddDefaultBalanceCheckSchedulerLockRepository()
                .AddDefaultBalanceRepository()
                .AddDefaultBalanceMonitoringTaskRepository()
                .AddDefaultBlockchainIndexationStateRepository()
                .AddDefaultBlockIndexationLockRepository()
                .AddDefaultTransactionReceiptRepository()
                .AddDefaultTransactionRepository()
                .AddDefaultTransactionMonitoringTaskRepository();
            
            builder
                .RegisterServices
                (
                    jobSettings.CurrentValue.BlockLockDuration,
                    jobSettings.Nested(x => x.ConfirmationLevel),
                    jobSettings.Nested(x => x.GasPriceRange),
                    jobSettings.CurrentValue.IndexOnlyOwnTransactions,
                    maximalBalanceCheckPeriod,
                    BigInteger.Parse(jobSettings.CurrentValue.MinBlockNumberToIndex)
                )
                .AddDefaultBalanceCheckSchedulingService()
                .AddDefaultBalanceMonitoringService()
                .AddDefaultBlockchainIndexingService()
                .AddDefaultTransactionMonitoringService();
            
            builder
                .UseQueueConsumers
                (
                    jobSettings.CurrentValue.BalanceMonitoringMaxDegreeOfParallelism,
                    jobSettings.CurrentValue.BlockchainIndexationMaxDegreeOfParallelism,
                    jobSettings.CurrentValue.TransactionMonitoringMaxDegreeOfParallelism
                )
                .AddBalanceMonitoringQueueConsumer()
                .AddBlockchainIndexationQueueConsumer()
                .AddTransactionMonitoringQueueConsumer();

            if (maximalBalanceCheckPeriod.HasValue)
            {
                builder.RegisterType<DefaultBalanceCheckScheduler>()
                    .IfNotRegistered(typeof(IBalanceCheckScheduler))
                    .As<IStartable>()
                    .As<IStopable>()
                    .SingleInstance();
            }
        }
    }
}