using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;

namespace Lykke.Quintessence.Validators
{
    [UsedImplicitly]
    public class WhitelistAddressRequestValidator : AbstractValidator<WhitelistAddressRequest>
    {
        public WhitelistAddressRequestValidator(
            IAddressService addressService)
        {
            RuleFor(x => x.Address)
                .AddressMustBeValid(addressService);
            
            RuleFor(x => x.MaxGasAmount)
                .AmountMustBeValid();
        }
    }
}
