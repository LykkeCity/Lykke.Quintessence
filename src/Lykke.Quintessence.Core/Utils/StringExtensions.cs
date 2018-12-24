using System.Text;

namespace Lykke.Quintessence.Core.Utils
{
    public static class StringExtensions
    {
        public static string ToHex(
            this string str)
        {
            return Encoding.UTF8.GetBytes(str).ToHexString();
        }
    }
}