using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultValidateChecksumStrategy : IValidateChecksumStrategy
    {
        private readonly IAddChecksumStrategy _addChecksumStrategy;

        
        public DefaultValidateChecksumStrategy(
            IAddChecksumStrategy addChecksumStrategy)
        {
            _addChecksumStrategy = addChecksumStrategy;
        }

        
        public async Task<bool> ExecuteAsync(
            string address)
        {
            var unprefixedAddress = address.Remove(0, 2);
            
            return unprefixedAddress == unprefixedAddress.ToLowerInvariant()
                || unprefixedAddress == unprefixedAddress.ToUpperInvariant()
                || address == (await _addChecksumStrategy.ExecuteAsync(address));
        }
    }
}