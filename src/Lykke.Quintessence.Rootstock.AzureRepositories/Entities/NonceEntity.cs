using System.Numerics;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Quintessence.Domain.Repositories.Entities
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class NonceEntity : AzureTableEntity
    {
        private BigInteger _value;
        
        
        public BigInteger Value
        {
            get
                => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;

                    MarkValueTypePropertyAsDirty(nameof(Value));
                }
            }
        }
    }
}