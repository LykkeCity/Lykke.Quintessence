using System;
using System.Numerics;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public abstract class GasAmountCalculationResult
    {
        public static ErrorResult Error(string message)
            => new ErrorResult(message);
        
        public static GasAmountResult GasAmount(BigInteger value)
            => new GasAmountResult(value);
        
        
        public static implicit operator BigInteger(
            GasAmountCalculationResult result)
        {
            if (result is GasAmountResult gasAmount)
            {
                return (BigInteger) gasAmount;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
        
        
        public class ErrorResult : GasAmountCalculationResult
        {
            private readonly string _message;
            
            internal ErrorResult(
                string message)
            {
                _message = message;
            }

            public override string ToString()
            {
                return _message;
            }
        }

        public class GasAmountResult : GasAmountCalculationResult
        {
            private readonly BigInteger _value;
            
            internal GasAmountResult(
                BigInteger value)
            {
                _value = value;
            }
            
            public static implicit operator BigInteger(
                GasAmountResult gasAmount)
            {
                return gasAmount._value;
            }
        }
    }
}