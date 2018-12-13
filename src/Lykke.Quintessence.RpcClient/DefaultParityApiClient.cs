using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient.Models;
using Lykke.Quintessence.RpcClient.Strategies;
using Lykke.Quintessence.RpcClient.Utils;

namespace Lykke.Quintessence.RpcClient
{
    [UsedImplicitly]
    public class DefaultParityApiClient : ApiClientBase, IParityApiClient
    {
        public DefaultParityApiClient(
            ISendRpcRequestStrategy sendRpcRequestStrategy)
        
            : base(sendRpcRequestStrategy)
        {
            
        }
        
        public async Task<BigInteger> GetNextNonceAsync(
            string address)
        {
            var requestParams = new[] { address };
            var request = new RpcRequest("eth_getTransactionCount", requestParams);
            var response = await SendRpcRequestAsync(request);
            
            return response.ResultValue<BigInteger>();
        }

        public async Task<IEnumerable<TransactionTrace>> GetTransactionTraces(
            string transactionHash)
        {
            var requestParams = new[] { transactionHash };
            var request = new RpcRequest("trace_transaction", requestParams);
            var response = await SendRpcRequestAsync(request);
            
            return response.ResultValue<IEnumerable<TransactionTrace>>();
        }
    }
}