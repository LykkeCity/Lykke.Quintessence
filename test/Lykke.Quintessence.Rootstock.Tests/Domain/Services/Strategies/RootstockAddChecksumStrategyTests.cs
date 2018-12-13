using System.Threading.Tasks;
using FluentAssertions;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [TestClass]
    public class RootstockAddChecksumStrategyTests
    {
        // ReSharper disable StringLiteralTypo
        [DataRow("0x5aaEB6053f3e94c9b9a09f33669435E7ef1bEAeD")]
        [DataRow("0xFb6916095cA1Df60bb79ce92cE3EA74c37c5d359")]
        [DataRow("0xD1220A0Cf47c7B9BE7a2e6ba89F429762E7B9adB")]
        [DataRow("0xDBF03B407c01E7CD3cBea99509D93F8Dddc8C6FB")]
        [DataTestMethod]
        // ReSharper restore StringLiteralTypo
        public async Task ExecuteAsync__Should_Calculate_Correct_Checksum_For_MainNet(
            string addressWithChecksum)
        {
            var chainId = new ChainId(30);
            var strategy = new RootstockAddChecksumStrategy(chainId, HashCalculator.Keccak256);
            var rawAddress = addressWithChecksum.ToLowerInvariant();

            (await strategy.ExecuteAsync(rawAddress))
                .Should()
                .Be(addressWithChecksum);
        }
        
        // ReSharper disable StringLiteralTypo
        [DataRow("0x5aAeb6053F3e94c9b9A09F33669435E7EF1BEaEd")]
        [DataRow("0xFb6916095CA1dF60bb79CE92ce3Ea74C37c5D359")]
        [DataRow("0xdbF03B407C01E7cd3cbEa99509D93f8dDDc8C6fB")]
        [DataRow("0xd1220a0CF47c7B9Be7A2E6Ba89f429762E7b9adB")]
        [DataTestMethod]
        // ReSharper restore StringLiteralTypo   
        public async Task ExecuteAsync__Should_Calculate_Correct_Checksum_For_TestNet(
            string addressWithChecksum)
        {
            var chainId = new ChainId(31);
            var strategy = new RootstockAddChecksumStrategy(chainId, HashCalculator.Keccak256);
            var rawAddress = addressWithChecksum.ToLowerInvariant();
            
            (await strategy.ExecuteAsync(rawAddress))
                .Should()
                .Be(addressWithChecksum);
        }
        
        // ReSharper disable StringLiteralTypo
        [DataRow("0x5aaEB6053f3e94c9b9a09f33669435E7ef1bEAeD")]
        [DataRow("0xFb6916095cA1Df60bb79ce92cE3EA74c37c5d359")]
        [DataRow("0xD1220A0Cf47c7B9BE7a2e6ba89F429762E7B9adB")]
        [DataRow("0xDBF03B407c01E7CD3cBea99509D93F8Dddc8C6FB")]
        [DataRow("0x5aAeb6053F3e94c9b9A09F33669435E7EF1BEaEd")]
        [DataRow("0xFb6916095CA1dF60bb79CE92ce3Ea74C37c5D359")]
        [DataRow("0xdbF03B407C01E7cd3cbEa99509D93f8dDDc8C6fB")]
        [DataRow("0xd1220a0CF47c7B9Be7A2E6Ba89f429762E7b9adB")]
        [DataTestMethod]
        // ReSharper restore StringLiteralTypo
        public async Task ExecuteAsync__Should_Calculate_InCorrect_Checksum(
            string addressWithChecksum)
        {
            var chainId = new ChainId(42);
            var strategy = new RootstockAddChecksumStrategy(chainId, HashCalculator.Keccak256);
            var rawAddress = addressWithChecksum.ToLowerInvariant();
            
            (await strategy.ExecuteAsync(rawAddress))
                .Should()
                .NotBe(addressWithChecksum);
        }
    }
}