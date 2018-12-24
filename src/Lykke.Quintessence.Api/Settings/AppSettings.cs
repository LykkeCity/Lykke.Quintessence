using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Sdk.Settings;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Quintessence.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings<T> : BaseAppSettings
        where T : ApiSettings
    {
        public T Api { get; set; }
        
        [Optional]
        public ChaosSettings Chaos { get; set; }
    }
}