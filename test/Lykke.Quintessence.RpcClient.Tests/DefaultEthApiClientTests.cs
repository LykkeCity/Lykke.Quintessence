using System.Numerics;
using System.Threading.Tasks;
using FluentAssertions;
using Lykke.Quintessence.RpcClient.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Quintessence.RpcClient
{
    [TestClass]
    public class DefaultEthApiClientTests
    {
        [TestMethod]
        public async Task EstimateGasAmountAsync()
        {
            var response = await GetClient("eth_estimateGas.json")
                .EstimateGasAmountAsync("0xc94770007dda54cF92009BFF0dE90c06F603a09f", "0xa94f5374fce5edbc8e2a8697c15331677e6ebf0b", 42);

            response
                .Should()
                .Be(21000);
        }
        
        [TestMethod]
        public async Task GetBalanceAsync()
        {
            var response = await GetClient("eth_getBalance/latest.json", "eth_getBalance.json")
                .GetBalanceAsync("0xc94770007dda54cF92009BFF0dE90c06F603a09f");

            response
                .Should()
                .Be(BigInteger.Parse("158972490234375000"));
        }
        
        [TestMethod]
        public async Task GetBalanceAsync_Block_Number_Passed()
        {
            var response = await GetClient("eth_getBalance.json")
                .GetBalanceAsync("0xc94770007dda54cF92009BFF0dE90c06F603a09f", 232);

            response
                .Should()
                .Be(BigInteger.Parse("158972490234375000"));
        }

        [TestMethod]
        public async Task GetBestBlockNumberAsync()
        {
            var response = await GetClient("eth_blockNumber.json")
                .GetBestBlockNumberAsync();

            response
                .Should()
                .Be(new BigInteger(815294));
        }

        [TestMethod]
        public async Task GetBlockAsync()
        {
            var response = await GetClient("eth_getBlockByNumber/latest.json", "block.json")
                .GetBlockAsync(false);
            
            ValidateBlock(response, false);
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Transactions_Requested()
        {
            var response = await GetClient("eth_getBlockByNumber/latest_with_transactions.json", "block_with_transactions.json")
                .GetBlockAsync(true);
            
            ValidateBlock(response, true);
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Hash_Passed()
        {
            var response = await GetClient("eth_getBlockByHash.json", "block.json")
                .GetBlockAsync("0x35c9a41ae1380141d1163c3ada714a924842fb7519f1a07ae2c21a94d2fa84be", false);
            
            ValidateBlock(response, false);
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Hash_Passed_And_Transactions_Requested()
        {
            var response = await GetClient("eth_getBlockByHash/with_transactions.json", "block_with_transactions.json")
                .GetBlockAsync("0x35c9a41ae1380141d1163c3ada714a924842fb7519f1a07ae2c21a94d2fa84be", true);
            
            ValidateBlock(response, true);
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Number_Passed()
        {
            var response = await GetClient("eth_getBlockByNumber.json", "block.json")
                .GetBlockAsync(816669, false);
            
            ValidateBlock(response, false);
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Number_Passed_And_Transactions_Requested()
        {
            var response = await GetClient("eth_getBlockByNumber/with_transactions.json", "block_with_transactions.json")
                .GetBlockAsync(816669, true);
            
            ValidateBlock(response, true);
        }

        [TestMethod]
        public async Task GetCodeAsync()
        {
            var response = await GetClient("eth_getCode.json")
                .GetCodeAsync("0xa94f5374fce5edbc8e2a8697c15331677e6ebf0b");

            response
                .Should()
                .Be("0x600160008035811a818181146012578301005b601b6001356025565b8060005260206000f25b600060078202905091905056");
        }

        [TestMethod]
        public async Task GetGasPriceAsync()
        {
            var response = await GetClient("eth_gasPrice.json")
                .GetGasPriceAsync();

            response
                .Should()
                .Be(BigInteger.Parse("10000000000000"));
        }

        [TestMethod]
        public async Task GetTransactionAsync()
        {
            var response = await GetClient("eth_getTransactionByHash.json")
                .GetTransactionAsync("0x88df016429689c079f3b2f6ad39fa052532c56795b733da78a91ebe6a713944b");
            
            var expectedTransaction = new Transaction
            (
                blockHash: "0x1d59ff54b1eb26b013ce3cb5fc9dab3705b415a67127a003c3e61eb445bb8df2",
                blockNumber: 6139707,
                from: "0xa7d9ddbe1f17865597fbd27ec712455208b6b76d",
                gas: 50000,
                gasPrice: 20000000000,
                input: "0x68656c6c6f21",
                nonce: 21,
                to: "0xf02c1c8e6114b1dbe8937a39260b5b0a374432bb",
                transactionIndex: 65,
                transactionHash: "0x88df016429689c079f3b2f6ad39fa052532c56795b733da78a91ebe6a713944b",
                value: 4290000000000000
            );
            
            response
                .Should()
                .BeEquivalentTo(expectedTransaction);
        }

        [TestMethod]
        public async Task GetTransactionCountAsync__Include_Pending()
        {
            var response = await GetClient("eth_getTransactionCount/pending.json", "eth_getTransactionCount.json")
                .GetTransactionCountAsync("0xc94770007dda54cF92009BFF0dE90c06F603a09f", true);

            response
                .Should()
                .Be(1);
        }
        
        [TestMethod]
        public async Task GetTransactionCountAsync__Not_Include_Pending()
        {
            var response = await GetClient("eth_getTransactionCount.json")
                .GetTransactionCountAsync("0xc94770007dda54cF92009BFF0dE90c06F603a09f", false);

            response
                .Should()
                .Be(1);
        }

        [TestMethod]
        public async Task GetTransactionReceiptAsync__Parity()
        {
            var response = await GetClient("eth_getTransactionReceipt.json", "eth_getTransactionReceipt/parity.json")
                .GetTransactionReceiptAsync("0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238");
            
            var expectedTransactionReceipt = new TransactionReceipt
            (
                blockHash: "0xc6ef2fc5426d6ad6fd9e2a26abeab0aa2411b7ab17f30a99d3cb96aed1d1055b",
                blockNumber: 11,
                contractAddress: "0xb60e8dd61c5d32be8058bb8eb970870f07233155",
                cumulativeGasUsed: 13244,
                gasUsed: 1244,
                status: 1,
                transactionIndex: 1,
                transactionHash: "0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238"
            );
            
            response
                .Should()
                .BeEquivalentTo(expectedTransactionReceipt);
        }
        
        [TestMethod]
        public async Task GetTransactionReceiptAsync__RSKJ()
        {
            var response = await GetClient("eth_getTransactionReceipt.json", "eth_getTransactionReceipt/rskj.json")
                .GetTransactionReceiptAsync("0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238");
            
            var expectedTransactionReceipt = new TransactionReceipt
            (
                blockHash: "0x93a626d7b09c34abca7c6bc82961a2462b2304a89c35ec8c9617da6fdafcaafa",
                blockNumber: 246058,
                contractAddress: null,
                cumulativeGasUsed: 105000,
                gasUsed: 21000,
                status: 1,
                transactionIndex: 4,
                transactionHash: "0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238"
            );
            
            response
                .Should()
                .BeEquivalentTo(expectedTransactionReceipt);
        }

        [TestMethod]
        public async Task SendRawTransactionAsync()
        {
            var response = await GetClient("eth_sendRawTransaction.json")
                .SendRawTransactionAsync("0xd46e8dd67c5d32be8d46e8dd67c5d32be8058bb8eb970870f072445675058bb8eb970870f072445675");

            response
                .Should()
                .Be("0xe670ec64341771606e55d6b4ca35a1a6b75ee3d5145a99d05921026d1527331");
        }
        
        private static IApiClient GetClient(
            string requestAndResponseSubPath)
        {
            return new DefaultApiClient
            (
                new SendRpcRequestStrategy(requestAndResponseSubPath, true)
            );
        }

        private static void ValidateBlock(
            Block block,
            bool blockShouldContainTransactions)
        {

            block
                .Number
                .Should()
                .NotBeNull();
            
            // ReSharper disable once PossibleInvalidOperationException
            block
                .Number
                .Value
                .Should()
                .Be(816669);
            
            block
                .Timestamp
                .Should()
                .Be(816670);

            block
                .BlockHash
                .Should()
                .Be("0x35c9a41ae1380141d1163c3ada714a924842fb7519f1a07ae2c21a94d2fa84be");

            block
                .ParentHash
                .Should()
                .Be("0x01a73fd568ed1d5ae736dac4a6dd71eb0e4fea311928bcc230385e422ac52b6a");

            block
                .TransactionHashes
                .Should()
                .BeEquivalentTo
                (
                    "0xd312d4495e37c95c52590b99f27e610d486429fd8438288c7db1032cf2a4c888",
                    "0x88df016429689c079f3b2f6ad39fa052532c56795b733da78a91ebe6a713944b"
                );
            
            block
                .TransactionHashes
                .Length
                .Should()
                .Be(2);
            
            if (blockShouldContainTransactions)
            {
                block
                    .Transactions
                    .Should()
                    .NotBeNull();

                
                // ReSharper disable once PossibleInvalidOperationException
                var transactions = block.Transactions.Value;
                
                transactions
                    .Length
                    .Should()
                    .Be(2);

                var actualTransaction = transactions[1];
                var expectedTransaction = new Transaction
                (
                    blockHash: "0x35c9a41ae1380141d1163c3ada714a924842fb7519f1a07ae2c21a94d2fa84be",
                    blockNumber: 816669,
                    from: "0xa7d9ddbe1f17865597fbd27ec712455208b6b76d",
                    gas: 50000,
                    gasPrice: 20000000000,
                    input: "0x0",
                    nonce: 21,
                    to: "0xf02c1c8e6114b1dbe8937a39260b5b0a374432bb",
                    transactionIndex: 65,
                    transactionHash: "0x88df016429689c079f3b2f6ad39fa052532c56795b733da78a91ebe6a713944b",
                    value: 4290000000000000
                );
                
                actualTransaction
                    .Should()
                    .BeEquivalentTo(expectedTransaction);
            }
            else
            {
                block
                    .Transactions
                    .Should()
                    .BeNull();
            }
        }
        
        private static IApiClient GetClient(
            string requestSubPath,
            string responseSubPath)
        {
            return new DefaultApiClient
            (
                new SendRpcRequestStrategy(requestSubPath, responseSubPath)
            );
        }
    }
}