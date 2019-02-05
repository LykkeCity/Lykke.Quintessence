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
                .Must((rootObject, amount, context) =>
                {
                    if (string.IsNullOrWhiteSpace(amount))
                    {
                        context.MessageFormatter.AppendArgument("Reason", "Must be specified.");

                        return false;
                    }

                    if (!BigInteger.TryParse(amount, out var amountParsed) || amountParsed <= 0)
                    {
                        context.MessageFormatter.AppendArgument("Reason", "Must be greater than zero.");

                        return false;
                    }

                    return true;
                })
                .WithMessage("{Reason}");
        }

        public static void AddressMustBeValid<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder,
            IAddressService addressService)
        {
            ruleBuilder
                .MustAsync(async (rootObject, address, context, cancellationToken) =>
                {
                    if (address == null)
                    {
                        context.MessageFormatter.AppendArgument("Reason", "Must be specified.");

                        return false;
                    }
                    
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

        public static void AssetMustBeValidAndSupported<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder,
            IAssetService assetService)
        {
            ruleBuilder
                .Must((rootObject, assetId, context) =>
                {
                    if (string.IsNullOrWhiteSpace(assetId))
                    {
                        context.MessageFormatter.AppendArgument("Reason", "Must be specified.");

                        return false;
                    }

                    if (assetService.Get().Id != assetId)
                    {
                        context.MessageFormatter.AppendArgument("Reason", "Must be supported.");

                        return false;
                    }

                    return true;
                })
                .WithMessage("{Reason}");
        }
        
        public static void TransactionIdMustBeNonEmptyGuid<T>(
            this IRuleBuilderInitial<T, Guid> ruleBuilder)
        {
            ruleBuilder
                .Must(transactionId => transactionId != Guid.Empty)
                .WithMessage(x => "Must not be empty.");
        }

        public static void MustBeHexString<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            ruleBuilder
                .Must(@string => @string != null && HexStringExpression.IsMatch(@string))
                .WithMessage(x => "Must be a hex string.");
        }
    }
}
