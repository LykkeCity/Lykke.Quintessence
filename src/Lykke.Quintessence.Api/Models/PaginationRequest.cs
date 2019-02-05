using JetBrains.Annotations;

namespace Lykke.Quintessence.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PaginationRequest
    {
        public PaginationRequest()
        {
            Continuation = string.Empty;
        }

        public string Continuation { get; set; }

        public int Take { get; set; }
    }
}
