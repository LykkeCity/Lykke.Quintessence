using System;
using System.Linq;
using System.Numerics;

namespace Lykke.Quintessence.Core.Utils
{
    public static class BigIntegerExtensions
    {
        public static string ToHexString(
            this BigInteger bigInteger)
        {
            if (bigInteger.Sign < 0)
            {
                throw new NotSupportedException($"Conversion of negative BigInteger [{bigInteger}] to hex string is not supported.");
            }

            if (bigInteger == 0)
            {
                return "0x0";
            }
            
            var hexBytes = bigInteger.ToByteArray().ToList();

            if (BitConverter.IsLittleEndian)
            {
                hexBytes.Reverse();
            }

            var hexString = string.Concat(hexBytes.Select(b => b.ToString("x2")));
            
            return $"0x{hexString.TrimStart('0')}";
        }
    }
}
