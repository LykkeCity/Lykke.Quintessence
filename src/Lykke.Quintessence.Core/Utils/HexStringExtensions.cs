using System;
using System.Linq;
using System.Numerics;
using System.Text;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Core.Utils
{
    [PublicAPI]
    public static class HexStringExtensions
    {
        public static BigInteger HexToBigInteger(
            this string hexString)
        {
            if (BitConverter.IsLittleEndian)
            {
                var hexBytes = hexString.HexToByteArray().ToList();
                    
                hexBytes.Insert(0, 0x00);
                hexBytes.Reverse();
                    
                return new BigInteger
                (
                    hexBytes.ToArray()
                );
            }
            else
            {
                return new BigInteger
                (
                    hexString.HexToByteArray()
                );
            }
        }
        
        public static byte[] HexToByteArray(
            this string hexString)
        {
            byte[] bytes;
            
            if (string.IsNullOrEmpty(hexString))
            {
                bytes = Array.Empty<byte>();
            }
            else
            {
                if (hexString.StartsWith("0x", StringComparison.Ordinal))
                {
                    hexString = hexString.Remove(0, 2);
                }

                if (hexString.Length % 2 != 0)
                {
                    hexString = $"0{hexString}";
                }

                bytes = new byte[hexString.Length / 2];
                
                for (var i = 0; i < hexString.Length; i += 2)
                {
                    var upperByte = hexString[i].ToByteWithShift(4);
                    var lowerByte = hexString[i + 1].ToByteWithShift(0);
                    
                    bytes[i / 2] = (byte) (upperByte | lowerByte);
                }
            }

            return bytes;
        }

        public static BigInteger? HexToNullableBigInteger(
            this string hexString)
        {
            if (!string.IsNullOrEmpty(hexString))
            {
                return hexString.HexToBigInteger();
            }
            else
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string HexToUTF8String(
            this string hexString)
        {
            var stringBytes = hexString.HexToByteArray();
            
            return Encoding.UTF8.GetString(stringBytes, 0, stringBytes.Length);
        }
        
        private static byte ToByteWithShift(
            this char character,
            int shift)
        {
            var value = (byte) character;
            
            if (0x40 < value && 0x47 > value || 0x60 < value && 0x67 > value)
            {
                if (0x40 == (0x40 & value))
                {
                    if (0x20 == (0x20 & value))
                    {
                        value = (byte) ((value + 0xA - 0x61) << shift);
                    }
                    else
                    {
                        value = (byte) ((value + 0xA - 0x41) << shift);
                    }
                }
            }
            else if (0x29 < value && 0x40 > value)
            {
                value = (byte) ((value - 0x30) << shift);
            }
            else
            {
                throw new InvalidOperationException($"Character '{character}' is not valid alphanumeric character.");
            }

            return value;
        }
    }
}
