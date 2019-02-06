using System.Threading.Tasks;
using Lykke.Quintessence;
using Lykke.Quintessence.Settings;

namespace Lykke.BilJob.SampleJob
{
    internal static class Program
    {
        public static Task Main(string[] args)
        {
            #if DEBUG
            
            return JobStarter.StartAsync<Startup, JobSettings>(true);
            
            #else

            return JobStarter.StartAsync<Startup, JobSettings>(false);
    
            #endif
        }
    }
}