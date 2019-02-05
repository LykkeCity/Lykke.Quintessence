using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Sdk.Settings;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Quintessence.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings<T> : BaseAppSettings
        where T : JobSettings
    {
                
        [Optional]
        public ChaosSettings Chaos { get; set; }
        
        public T Job { get; set; }
    }
}