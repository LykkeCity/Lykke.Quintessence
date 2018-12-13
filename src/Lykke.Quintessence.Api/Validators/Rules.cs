using System;
using System.Numerics;
using System.Text.RegularExpressions;
using FluentValidation;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Utils;

namespace Lykke.Quintessence.Validators
{
    public static class Rules
    {
        private static readonly Regex HexStringExpression
            = new Regex(@"^0x[0-9a-fA-F]+$", RegexOptions.Compiled);
        
        
        public static void AmountMustBeValid<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            ruleBuilder
                .Must(amount => BigInteger.TryParse(amount, out var amountParsed) && amountParsed > 0)
                .WithMessage(x => "Amount must be greater than zero.");
        }

        public static void AddressMustBeValid<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder,
            IAddressService addressService)
        {
            ruleBuilder
                .MustAsync(async (rootObject, address, context, cancellationToken) =>
                {
                    var validationResult = await addressService.ValidateAsync(address, false, false);

                    if (validationResult is AddressValidationResult.Error error)
                    {
                        
                        context.MessageFormatter.AppendArgument("Reason", error.Type.ToReason());
                        
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                })
                .WithMessage("{Reason}");
        }

        public static void AddressMustBeProperlyFormatted<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder,
            IAddressService addressService)
        {
            ruleBuilder
                .MustAsync(async (rootObject, address, context, cancellationToken) =>
                {
                    var validationResult = await addressService.ValidateAsync(address, true, true);

                    if (validationResult is AddressValidationResult.Error error)
                    {
                        context.MessageFormatter.AppendArgument("Reason", error.Type.ToReason());
                        
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                })
                .WithMessage("{Reason}");
        }

        public static void AssetMustBeSupported<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder,
            IAssetService assetService)
        {
            ruleBuilder
                .Must(assetId => assetService.Get().Id == assetId)
                .WithMessage("Asset is not supported.");
        }
        
        public static void TransactionIdMustBeNonEmptyGuid<T>(
            this IRuleBuilderInitial<T, Guid> ruleBuilder)
        {
            ruleBuilder
                .Must(transactionId => transactionId != Guid.Empty)
                .WithMessage(x => "Specified transaction id is empty.");
        }

        public static void MustBeHexString<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            ruleBuilder
                .Must(@string => HexStringExpression.IsMatch(@string))
                .WithMessage(x => "Must be a hex string.");
        }
    }
}
