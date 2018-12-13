using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Models;

namespace Lykke.Quintessence.Validators
{
    [UsedImplicitly]
    public class OperationRequestValidator : AbstractValidator<TransactionRequest>
    {
        public OperationRequestValidator()
        {
            RuleFor(x => x.TransactionId)
                .TransactionIdMustBeNonEmptyGuid();
        }
    }
}
