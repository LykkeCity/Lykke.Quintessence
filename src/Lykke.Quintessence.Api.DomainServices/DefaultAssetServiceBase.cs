using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public abstract class DefaultAssetServiceBase : IAssetService
    {
        private readonly Asset _asset;
        
        
        protected DefaultAssetServiceBase(
            BigInteger assetAccuracy,
            string assetAddress,
            string assetId,
            string assetName)
        {
            _asset = new Asset
            (
                accuracy: assetAccuracy,
                address: assetAddress,
                id: assetId,
                name: assetName
            );
        }
        
        public Asset Get()
        {
            return _asset;
        }
    }
}