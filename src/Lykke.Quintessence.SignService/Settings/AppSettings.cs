using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Quintessence.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings<T> : BaseAppSettings
        where T : SignServiceSettings
    {
        [Optional]
        public T SignService { get; set; }
    }
}