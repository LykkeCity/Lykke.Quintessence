using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AssetRequest
    {
        [FromRoute]
        public string AssetId { get; set; }
    }
}
