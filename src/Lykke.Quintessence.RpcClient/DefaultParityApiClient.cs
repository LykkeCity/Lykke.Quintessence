using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.RpcClient.Models;
using Lykke.Quintessence.RpcClient.Strategies;
using Lykke.Quintessence.RpcClient.Utils;
using Newtonsoft.Json.Linq;

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
            var requestParams = new object[] { address };
            var request = new RpcRequest("parity_nextNonce", requestParams);
            var response = await SendRpcRequestAsync(request);
            
            return response.ResultValue<BigInteger>();
        }

        public async Task<IEnumerable<TransactionTrace>> GetTransactionTraces(
            string transactionHash)
        {
            var requestParams = new object[] { transactionHash };
            var request = new RpcRequest("trace_transaction", requestParams);
            var response = await SendRpcRequestAsync(request);

            return response.Result.Value<JArray>()
                .Select(GetTransactionTrace);
        }
        
        private static TransactionTrace GetTransactionTrace(
            JToken jToken)
        {
            return new TransactionTrace
            (
                action: new TransactionTrace.TransactionAction
                (
                    callType: jToken.SelectToken("action.callType").Value<string>(),
                    from: jToken.SelectToken("action.from").Value<string>(),
                    to: jToken.SelectToken("action.to").Value<string>(),
                    value: jToken.SelectToken("action.value").Value<string>().HexToBigInteger()
                ),
                blockHash: jToken.Value<string>("blockHash"),
                blockNumber: jToken.Value<string>("blockNumber").HexToBigInteger(),
                error: jToken.Value<string>("error"),
                transactionHash: jToken.Value<string>("transactionHash"),
                type: jToken.Value<string>("type")
            );
        }
    }
}