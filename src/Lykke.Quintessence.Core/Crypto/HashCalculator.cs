using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Utils;
using Multiformats.Hash;

namespace Lykke.Quintessence.Core.Crypto
{
    [UsedImplicitly]
    public class HashCalculator : IHashCalculator
    {
        // ReSharper disable once InconsistentNaming
        public static HashCalculator Blake2b256
            => new HashCalculator(HashType.BLAKE2B_256);
        
        public static HashCalculator Keccak256
            => new HashCalculator(HashType.KECCAK_256);
        
        
        private readonly HashType _hashType;
        
        public HashCalculator(
            HashType hashType)
        {
            _hashType = hashType;
        }
        
        public byte[] Sum(
            params byte[][] data)
        {
            var multihash = Multihash.Sum
            (
                code: _hashType,
                data: data.Concat()
            );

            return multihash.Digest;
        }

        public async Task<byte[]> SumAsync(
            params byte[][] data)
        {
            var multihash = await Multihash.SumAsync
            (
                type: _hashType,
                data: data.Concat()
            );

            return multihash.Digest;
        }
    }
}