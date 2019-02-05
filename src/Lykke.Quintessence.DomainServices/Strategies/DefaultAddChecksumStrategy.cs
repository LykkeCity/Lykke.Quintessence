using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Crypto;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultAddChecksumStrategy : IAddChecksumStrategy
    {
        private readonly IHashCalculator _hashCalculator;
        
        
        public DefaultAddChecksumStrategy(
            IHashCalculator hashCalculator)
        {
            _hashCalculator = hashCalculator;
        }
        
        
        public async Task<string> ExecuteAsync(
            string address)
        {
            address = address.Remove(0, 2).ToLowerInvariant();

            var addressBytes = Encoding.UTF8.GetBytes(address);
            var caseMapBytes = await _hashCalculator.SumAsync(addressBytes);
                
            var addressBuilder = new StringBuilder("0x");
                
            for (var i = 0; i < 40; i++)
            {
                var addressChar = address[i];
                
                if (char.IsLetter(addressChar))
                {
                    var leftShift = i % 2 == 0 ? 7 : 3;
                    var shouldBeUpper = (caseMapBytes[i / 2] & (1 << leftShift)) != 0;

                    if (shouldBeUpper)
                    {
                        addressChar = char.ToUpper(addressChar);
                    }
                }
                    
                addressBuilder.Append(addressChar);
            }

            return addressBuilder.ToString();
        }
    }
}