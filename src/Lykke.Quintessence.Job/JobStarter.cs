using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Settings;
using Lykke.Sdk;

namespace Lykke.Quintessence
{
    [PublicAPI]
    public static class JobStarter
    {
        public static bool IsDebug { get; private set; }
        
        
        public static Task StartAsync<TStartup, TSettings>(
            bool isDebug)
            where TStartup : JobStartupBase<TSettings>
            where TSettings : JobSettings
        {
            IsDebug = isDebug;
            
            return LykkeStarter.Start<TStartup>(IsDebug);
        }
    }
}