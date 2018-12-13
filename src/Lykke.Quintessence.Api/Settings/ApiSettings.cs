using JetBrains.Annotations;

namespace Lykke.Quintessence.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ApiSettings
    {
        public int ConfirmationLevel { get; set; }
        
        public string GasPriceRange { get; set; }
        
        public int GasReserve { get; set; }
        
        public string MaxGasAmount { get; set; }
        
        public RpcNodeSettings RpcNode { get; set; }
        
        public DbSettings Db { get; set; }
    }
}