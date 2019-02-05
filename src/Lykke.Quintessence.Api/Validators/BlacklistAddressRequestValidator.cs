using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;

namespace Lykke.Quintessence.Validators
{
    [UsedImplicitly]
    public class BlacklistAddressRequestValidator : AbstractValidator<BlacklistAddressRequest>
    {
        public BlacklistAddressRequestValidator(
            IAddressService addressService)
        {
            RuleFor(x => x.Address)
                .AddressMustBeProperlyFormatted(addressService);

            RuleFor(x => x.BlacklistingReason)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
