using System;
using Autofac;
using Autofac.Core;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories.DependencyInjection
{
    [PublicAPI]
    public static class AzureRepositoriesRegistrantExtensions
    {
        public static IAzureRepositoriesRegistrant RegisterDefaultRepository<T>(
            this IAzureRepositoriesRegistrant registrant,
            Func<IReloadingManager<string>, T> repositoryBuilder)
        {
            registrant
                .Builder
                .Register(ctx => repositoryBuilder.Invoke
                (
                    registrant.ConnectionString
                ))
                .As<T>()
                .IfNotRegistered(typeof(T))
                .SingleInstance();

            return registrant;
        }
        
        public static IAzureRepositoriesRegistrant RegisterDefaultRepository<T>(
            this IAzureRepositoriesRegistrant registrant,
            Func<IReloadingManager<string>, ILogFactory, T> repositoryBuilder)
        {
            registrant
                .Builder
                .Register(ctx => repositoryBuilder.Invoke
                (
                    registrant.ConnectionString,
                    ctx.Resolve<ILogFactory>()
                ))
                .As<T>()
                .IfNotRegistered(typeof(T))
                .SingleInstance();

            return registrant;
        }

        public static IAzureRepositoriesRegistrant AddDefaultBalanceCheckSchedulerLockRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultBalanceCheckSchedulerLockRepository.Create);
        }
        
        public static IAzureRepositoriesRegistrant AddDefaultBalanceMonitoringTaskRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultBalanceMonitoringTaskRepository.Create);
        }
        
        public static IAzureRepositoriesRegistrant AddDefaultBalanceRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultBalanceRepository.Create);
        }

        public static IAzureRepositoriesRegistrant AddDefaultBlockchainIndexationStateRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultBlockchainIndexationStateRepository.Create);
        }
        
        public static IAzureRepositoriesRegistrant AddDefaultBlockIndexationLockRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultBlockIndexationLockRepository.Create);
        }
        
        public static IAzureRepositoriesRegistrant AddDefaultBlacklistedAddressRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultBlacklistedAddressRepository.Create);
        }

        public static IAzureRepositoriesRegistrant AddDefaultTransactionHistoryObservationAddressesRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultTransactionHistoryObservationAddressesRepository.Create);
        }
        
        public static IAzureRepositoriesRegistrant AddDefaultTransactionMonitoringTaskRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultTransactionMonitoringTaskRepository.Create);
        }

        public static IAzureRepositoriesRegistrant AddDefaultTransactionReceiptRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultTransactionReceiptRepository.Create);
        }

        public static IAzureRepositoriesRegistrant AddDefaultTransactionRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultTransactionRepository.Create);
        }

        public static IAzureRepositoriesRegistrant AddDefaultWhitelistedAddressRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(DefaultWhitelistedAddressRepository.Create);
        }
    }
}