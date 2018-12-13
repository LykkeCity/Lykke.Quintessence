namespace Lykke.Quintessence.Core.Blockchain
{
    public class DefaultRawTransaction
    {
        public DefaultRawTransaction(
            string data,
            string from,
            string hash)
        {
            Data = data;
            From = from;
            Hash = hash;
        }

        public string Data { get; }
        
        public string From { get; }
        
        public string Hash { get; }
    }
}