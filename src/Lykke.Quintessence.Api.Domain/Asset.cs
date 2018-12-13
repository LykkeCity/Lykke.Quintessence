using System.Numerics;

namespace Lykke.Quintessence.Domain
{
    public class Asset
    {
        public Asset(
            BigInteger accuracy,
            string address,
            string id,
            string name)
        {
            Accuracy = accuracy;
            Address = address;
            Id = id;
            Name = name;
        }

        
        public BigInteger Accuracy { get; }
        
        public string Address { get; }
        
        public string Id { get; }
        
        public string Name { get; }
    }
}