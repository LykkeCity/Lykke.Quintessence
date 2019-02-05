using System;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lykke.Quintessence.Validators
{
    [UsedImplicitly]
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator()
        {
            RuleFor(x => x.Take)
                .Must(take => take > 0)
                .WithMessage(x => "Take parameter must be greater than zero.");

            RuleFor(x => x.Continuation)
                .Must(ValidateContinuationToken)
                .WithMessage(x => "Continuation token is not valid.");
        }

        private static bool ValidateContinuationToken(
            string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var decodedToken = token.HexToUTF8String();

                    JsonConvert.DeserializeObject<TableContinuationToken>(decodedToken);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
