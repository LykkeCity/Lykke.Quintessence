using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AddressRequest
    {
        [FromRoute]
        public string Address { get; set; }
    }
}
