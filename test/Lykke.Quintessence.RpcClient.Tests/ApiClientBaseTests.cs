using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lykke.Quintessence.RpcClient.Exceptions;
using Lykke.Quintessence.RpcClient.Models;
using Lykke.Quintessence.RpcClient.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Quintessence.RpcClient
{
    [TestClass]
    public class ApiClientBaseTests
    {
        [TestMethod]
        public async Task SendRpcRequestAsync__Error_Returned__Exception_Thrown()
        {
            var request = new RpcRequest("test");
            var strategyMock = new SendRpcRequestStrategy("error.json", false);
            var clientMock = new ApiClient(strategyMock);

            RpcErrorException exception = null;
            
            try
            {
                await clientMock.SendRpcRequestAsync(request);
            }
            catch (RpcErrorException e)
            {
                exception = e;
            }

            exception
                .Should()
                .NotBeNull();

            if (exception != null)
            {
                exception
                    .ErrorMessage
                    .Should()
                    .Be("Execution error");

                exception
                    .ErrorCode
                    .Should()
                    .Be(3);

                exception
                    .Request
                    .Should()
                    .NotBeNullOrEmpty();
            }

        }
        
        private class ApiClient : ApiClientBase
        {
            public ApiClient(
                ISendRpcRequestStrategy sendRpcRequestStrategy) 
                
                : base(sendRpcRequestStrategy)
            {
                
            }

            public new Task<RpcResponse> SendRpcRequestAsync(
                RpcRequest request)
            {
                return base.SendRpcRequestAsync(request);
            }
        }
    }
}