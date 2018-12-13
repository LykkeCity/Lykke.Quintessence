using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Lykke.Quintessence.RpcClient.Models
{
    [JsonObject]
    [PublicAPI, ImmutableObject(true)]
    public sealed class RpcRequest
    {
        [JsonConstructor]
        public RpcRequest(
            string id,
            string jsonRpcVersion,
            string method,
            object[] parameters)
        {
            Id = id;
            JsonRpcVersion = jsonRpcVersion;
            Method = method;
            Parameters = parameters;
        }
        
        public RpcRequest(
            string method)
            
            : this(method, Array.Empty<string>())
        {
            
        }
        
        public RpcRequest(
            string method,
            object[] parameters)
        {
            Id = Guid.NewGuid().ToString();
            JsonRpcVersion = "2.0";
            Method = method;
            Parameters = parameters;
        }
        
        
        /// <summary>
        ///    Request id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }
        
        /// <summary>
        ///    Version of the JsonRpc to be used.
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string JsonRpcVersion { get; }
        
        /// <summary>
        ///    Name of the target method.
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; }
        
        /// <summary>
        ///    Parameters to invoke the method with.
        /// </summary>
        [JsonProperty("params")]
        public object[] Parameters { get; }
    }
}
