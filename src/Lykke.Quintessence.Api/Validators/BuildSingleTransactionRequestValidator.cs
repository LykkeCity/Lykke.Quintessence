using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Service.BlockchainApi.Contract.Transactions;

namespace Lykke.Quintessence.Validators
{
    [UsedImplicitly]
    public class BuildSingleTransactionRequestValidator : AbstractValidator<BuildSingleTransactionRequest>
    {
        public BuildSingleTransactionRequestValidator(
            IAddressService addressService,
            IAssetService assetService)
        {
            RuleFor(x => x.Amount)
                .AmountMustBeValid();

            RuleFor(x => x.FromAddress)
                .AddressMustBeValid(addressService);
            
            RuleFor(x => x.OperationId)
                .TransactionIdMustBeNonEmptyGuid();

            RuleFor(x => x.ToAddress)
                .AddressMustBeValid(addressService);

            RuleFor(x => x.AssetId)
                .AssetMustBeValidAndSupported(assetService);
        }
    }
}
