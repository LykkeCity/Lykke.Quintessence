using System.Threading.Tasks;
using Lykke.Quintessence.RpcClient.Exceptions;
using Lykke.Quintessence.RpcClient.Models;
using Lykke.Quintessence.RpcClient.Strategies;
using Newtonsoft.Json;

namespace Lykke.Quintessence.RpcClient
{
    public abstract class ApiClientBase
    {
        private readonly ISendRpcRequestStrategy _sendRpcRequestStrategy;

        
        protected ApiClientBase(
            ISendRpcRequestStrategy sendRpcRequestStrategy)
        {
            _sendRpcRequestStrategy = sendRpcRequestStrategy;
        }

        
        protected async Task<RpcResponse> SendRpcRequestAsync(
            RpcRequest request)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            var responseJson = await _sendRpcRequestStrategy.ExecuteAsync(requestJson);
            var response = JsonConvert.DeserializeObject<RpcResponse>(responseJson);
            
            if (response.Error != null)
            {
                var error = response.Error;
                
                throw new RpcErrorException(requestJson, error.Code, error.Message);
            }
            else
            {
                return response;
            }
        }
    }
}