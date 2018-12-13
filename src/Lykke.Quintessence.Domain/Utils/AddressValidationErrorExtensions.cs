using System;
using Lykke.Quintessence.Domain.Services;

namespace Lykke.Quintessence.Domain.Utils
{
    public static class AddressValidationErrorExtensions
    {
        public static string ToReason(
            this AddressValidationError error)
        {
            switch (error)
            {
                case AddressValidationError.AddressIsBlacklisted:
                    return "Address is blacklisted";
                
                case AddressValidationError.ChecksumIsInvalid:
                    return "Checksum is invalid";
                
                case AddressValidationError.FormatIsInvalid:
                    return "Format is invalid";
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }
    }
}