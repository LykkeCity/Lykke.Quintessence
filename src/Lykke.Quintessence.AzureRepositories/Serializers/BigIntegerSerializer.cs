﻿using System;
using System.Numerics;
using Lykke.AzureStorage.Tables.Entity.Serializers;

namespace Lykke.Quintessence.Domain.Repositories.Serializers
{
    public class BigIntegerSerializer : IStorageValueSerializer
    {
        public string Serialize(
            object value,
            Type type)
        {
            return ((BigInteger) value).ToString();
        }

        public object Deserialize(
            string serialized,
            Type type)
        {
            return BigInteger.Parse(serialized);
        }
    }
}
