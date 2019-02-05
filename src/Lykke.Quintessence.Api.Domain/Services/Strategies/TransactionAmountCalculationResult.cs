using System;
using System.Numerics;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public class TransactionAmountCalculationResult
    {
        public static BalanceIsNotEnoughResult BalanceIsNotEnough()
            => new BalanceIsNotEnoughResult();
        
        public static TransactionAmountIsTooSmallResult TransactionAmountIsTooSmall()
            => new TransactionAmountIsTooSmallResult();
        
        public static TransactionAmountResult TransactionAmount(BigInteger value)
            => new TransactionAmountResult(value);
        
        
        public static implicit operator BigInteger(
            TransactionAmountCalculationResult result)
        {
            if (result is TransactionAmountResult transactionAmount)
            {
                return (BigInteger) transactionAmount;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
        
        
        public class BalanceIsNotEnoughResult : TransactionAmountCalculationResult
        {
            internal BalanceIsNotEnoughResult()
            {
                
            }
        }
        
        public class TransactionAmountIsTooSmallResult : TransactionAmountCalculationResult
        {
            internal TransactionAmountIsTooSmallResult()
            {
                
            }
        }
        
        public class TransactionAmountResult : TransactionAmountCalculationResult
        {
            private readonly BigInteger _value;
            
            internal TransactionAmountResult(
                BigInteger value)
            {
                _value = value;
            }
            
            public static implicit operator BigInteger(
                TransactionAmountResult gasAmount)
            {
                return gasAmount._value;
            }
        }
    }
}