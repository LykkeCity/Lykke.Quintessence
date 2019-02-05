using System.Numerics;

namespace Lykke.Quintessence.Domain
{
    public class WhitelistedAddress
    {
        public WhitelistedAddress(
            string address,
            BigInteger maxGasAmount)
        {
            Address = address;
            MaxGasAmount = maxGasAmount;
        }

        public string Address { get; }
        
        public BigInteger MaxGasAmount { get; }
    }
}