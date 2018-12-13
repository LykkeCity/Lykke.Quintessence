using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Quintessence.RpcClient.Exceptions;

namespace Lykke.Quintessence.RpcClient.Strategies
{
    public class DefaultSendRpcRequestStrategy : ISendRpcRequestStrategy
    {
        private readonly Uri _apiUrl;
        private readonly TimeSpan _connectionTimeout;
        private readonly IHttpClientFactory _httpClientFactory;
        
        

        public DefaultSendRpcRequestStrategy(
            Uri apiUrl,
            TimeSpan connectionTimeout,
            IHttpClientFactory httpClientFactory)
        {
            _apiUrl = apiUrl;
            _connectionTimeout = connectionTimeout;
            _httpClientFactory = httpClientFactory;
        }


        public virtual async Task<string> ExecuteAsync(
            string requestJson)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var cts = new CancellationTokenSource();

                cts.CancelAfter(_connectionTimeout);
                
                var httpRequest = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PostAsync(_apiUrl, httpRequest, cts.Token);

                httpResponse.EnsureSuccessStatusCode();

                var responseJson = await httpResponse.Content.ReadAsStringAsync();
                
                return responseJson;
            }
            catch (TaskCanceledException)
            {
                throw new RpcTimeoutException(_connectionTimeout, requestJson);
            }
            catch (Exception e)
            {
                throw new RpcException
                (
                    "Error occurred while trying to send rpc request.",
                    requestJson,
                    e
                );
            }
        }
    }
}