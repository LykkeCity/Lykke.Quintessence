using System.Numerics;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.RpcClient.Models;
using Newtonsoft.Json.Linq;

namespace Lykke.Quintessence.RpcClient.Utils
{
    public static class RpcResponseExtensions
    {
        public static T ResultValue<T>(
            this RpcResponse rpcResponse)
            => ResultValue<T>(rpcResponse.Result);

        public static T ResultValue<T>(
            this RpcResponse rpcResponse,
            string key)
            => ResultValue<T>(rpcResponse.Result[key]);

        private static T ResultValue<T>(
            JToken result)
        {
            if (typeof(T) == typeof(BigInteger))
            {
                return (T) (object) result.Value<string>().HexToBigInteger();
            }
            else if (typeof(T) == typeof(BigInteger?))
            {
                return (T) (object) result.Value<string>().HexToNullableBigInteger();
            }
            else
            {
                return result.Value<T>();
            }
        }
    }
}
