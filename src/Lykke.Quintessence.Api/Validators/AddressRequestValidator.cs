using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;

namespace Lykke.Quintessence.Validators
{
    [UsedImplicitly]
    public class AddressRequestValidator : AbstractValidator<AddressRequest>
    {
        public AddressRequestValidator(
            IAddressService addressService)
        {
            RuleFor(x => x.Address)
                .AddressMustBeValid(addressService);
        }
    }
}
