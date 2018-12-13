using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;

namespace Lykke.Quintessence
{
    [PublicAPI]
    public static class SignServiceStarter
    {
        public static bool IsDebug { get; private set; }
        
        public static Task StartAsync<TStartup, TSettings>(
            bool isDebug)
            where TStartup : SignServiceStartupBase<TSettings>
            where TSettings : SignServiceSettings
        {
            IsDebug = isDebug;
            
            return LykkeStarter.Start<TStartup>(IsDebug);
        }
    }
}