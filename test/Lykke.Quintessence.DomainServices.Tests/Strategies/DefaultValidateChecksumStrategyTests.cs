using System.Threading.Tasks;
using FluentAssertions;
using Lykke.Quintessence.Domain.Services.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Quintessence.Domain.Services.Tests.Strategies
{
    [TestClass]
    public class DefaultValidateChecksumStrategyTests
    {
        [TestMethod]
        public async Task ExecuteAsync__Address_Chars_Are_All_Lower__True_Returned()
        {
            var addChecksumStrategyMock = new Mock<IAddChecksumStrategy>();
            
            var validateChecksumStrategy = new DefaultValidateChecksumStrategy(addChecksumStrategyMock.Object);

            // ReSharper disable StringLiteralTypo
            (await validateChecksumStrategy.ExecuteAsync("0x5aaeb6053f3e94c9b9a09f33669435e7ef1beaed"))
                .Should()
                .BeTrue();
            // ReSharper restore StringLiteralTypo
        }
        
        [TestMethod]
        public async Task ExecuteAsync__Address_Chars_Are_All_Upper__True_Returned()
        {
            var addChecksumStrategyMock = new Mock<IAddChecksumStrategy>();
            
            var validateChecksumStrategy = new DefaultValidateChecksumStrategy(addChecksumStrategyMock.Object);

            // ReSharper disable StringLiteralTypo
            (await validateChecksumStrategy.ExecuteAsync("0x5AAEB6053F3E94C9B9A09F33669435E7EF1BEAED"))
                .Should()
                .BeTrue();
            // ReSharper restore StringLiteralTypo
        }
        
        [TestMethod]
        public async Task ExecuteAsync__Address_Is_Equal_To_Address_With_Checksum__True_Returned()
        {
            var addChecksumStrategyMock = new Mock<IAddChecksumStrategy>();

            addChecksumStrategyMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>()))
                .ReturnsAsync("0x5aaEB6053f3e94c9b9a09f33669435E7ef1bEAeD");
            
            var validateChecksumStrategy = new DefaultValidateChecksumStrategy(addChecksumStrategyMock.Object);

            (await validateChecksumStrategy.ExecuteAsync("0x5aaEB6053f3e94c9b9a09f33669435E7ef1bEAeD"))
                .Should()
                .BeTrue();
        }
        
        [TestMethod]
        public async Task ExecuteAsync__Address_Is_Not_Equal_To_Address_With_Checksum__False_Returned()
        {
            var addChecksumStrategyMock = new Mock<IAddChecksumStrategy>();

            addChecksumStrategyMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>()))
                .ReturnsAsync("0x5aaEB6053f3e94c9b9a09f33669435E7ef1bEAed");
            
            var validateChecksumStrategy = new DefaultValidateChecksumStrategy(addChecksumStrategyMock.Object);

            (await validateChecksumStrategy.ExecuteAsync("0x5aaEB6053f3e94c9b9a09f33669435E7ef1bEAeD"))
                .Should()
                .BeFalse();
        }
    }
}