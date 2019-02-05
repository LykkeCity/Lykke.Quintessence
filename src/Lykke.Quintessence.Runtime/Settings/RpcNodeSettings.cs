using JetBrains.Annotations;

namespace Lykke.Quintessence.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RpcNodeSettings
    {
        public string ApiUrl { get; set; }
        
        public int ConnectionTimeout { get; set; }
        
        public bool EnableTelemetry { get; set; }
    }
}