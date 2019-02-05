using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Models;

namespace Lykke.Quintessence.Validators
{
    [UsedImplicitly]
    public class AssetRequestValidator : AbstractValidator<AssetRequest>
    {
        public AssetRequestValidator()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty();
        }
    }
}
