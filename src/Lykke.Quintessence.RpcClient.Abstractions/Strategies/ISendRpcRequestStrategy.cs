using System.Threading.Tasks;

namespace Lykke.Quintessence.RpcClient.Strategies
{
    public interface ISendRpcRequestStrategy
    {
        Task<string> ExecuteAsync(
            string requestJson);
    }
}