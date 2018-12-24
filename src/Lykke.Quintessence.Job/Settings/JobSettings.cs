using System;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Quintessence.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class JobSettings
    {
        public int BalanceMonitoringMaxDegreeOfParallelism { get; set; }
        
        public int BlockchainIndexationMaxDegreeOfParallelism { get; set; }
        
        public TimeSpan BlockLockDuration { get; set; }
        
        public int ConfirmationLevel { get; set; }
        
        public DbSettings Db { get; set; }
        
        public string GasPriceRange { get; set; }
        
        public bool IndexOnlyOwnTransactions { get; set; }
        
        public string MinBlockNumberToIndex { get; set; }
        
        public RpcNodeSettings RpcNode { get; set; }
        
        public int TransactionMonitoringMaxDegreeOfParallelism { get; set; }
    }
}