using System.Threading.Tasks;
using Lykke.Quintessence;
using Lykke.Quintessence.Settings;

namespace Lykke.BilService.SampleSignService
{
    internal static class Program
    {
        public static Task Main(string[] args)
        {
            #if DEBUG
            
            return SignServiceStarter.StartAsync<Startup, SignServiceSettings>(true);
            
            #else

            return SignServiceStarter.StartAsync<Startup, SignServiceSettings>(false);
    
            #endif
        }
    }
}