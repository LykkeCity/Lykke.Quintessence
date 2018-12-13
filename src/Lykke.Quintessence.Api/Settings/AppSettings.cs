using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Quintessence.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings<T> : BaseAppSettings
        where T : ApiSettings
    {
        public T Api { get; set; }
    }
}