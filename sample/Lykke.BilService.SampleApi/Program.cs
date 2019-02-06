using System.Threading.Tasks;
using Lykke.Quintessence;
using Lykke.Quintessence.Settings;

namespace Lykke.BilService.SampleApi
{
    internal static class Program
    {
        public static Task Main(string[] args)
        {
            #if DEBUG
            
            return ApiStarter.StartAsync<Startup, ApiSettings>(true);
            
            #else

            return ApiStarter.StartAsync<Startup, ApiSettings>(false);
    
            #endif
        }
    }
}