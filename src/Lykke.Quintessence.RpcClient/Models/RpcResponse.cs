using System.ComponentModel;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Quintessence.RpcClient.Models
{
    [JsonObject]
    [PublicAPI, ImmutableObject(true)]
    public sealed class RpcResponse
    {
        public RpcResponse(
            RpcError error,
            string id,
            JToken result)
        
            : this(error, id, "2.0", result)
        {
            
        }
        
        [JsonConstructor]
        public RpcResponse(
            RpcError error,
            string id,
            string jsonRpcVersion,
            JToken result)
        {
            Error = error;
            Id = id;
            JsonRpcVersion = jsonRpcVersion;
            Result = result;
        }
        
        
        /// <summary>
        ///    Request id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        ///    RPC request version.
        /// </summary>
        [JsonProperty("jsonrpc")] 
        public string JsonRpcVersion { get; }

        /// <summary>
        ///    Response result object.
        /// </summary>
        [JsonProperty("result")]
        public JToken Result { get; }

        /// <summary>
        ///    RPC request error.
        /// </summary>
        [JsonProperty("error")]
        public RpcError Error { get; }
    }
}
