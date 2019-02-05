using System;
using System.Numerics;

namespace Lykke.Quintessence.Domain
{
    public class Transaction
    {
        private Transaction(
            BigInteger amount,
            DateTime builtOn,
            string data,
            string from,
            BigInteger gasAmount,
            BigInteger gasPrice,
            bool includeFee,
            BigInteger nonce,
            TransactionState state,
            string to,
            Guid transactionId)
        {
            Amount = amount;
            BuiltOn = builtOn;
            Data = data;
            From = from;
            GasAmount = gasAmount;
            GasPrice = gasPrice;
            IncludeFee = includeFee;
            Nonce = nonce;
            State = state;
            To = to;
            TransactionId = transactionId;
        }

        public Transaction(
            BigInteger amount,
            BigInteger? blockNumber,
            DateTime? broadcastedOn,
            DateTime builtOn,
            DateTime? completedOn,
            BigInteger? confirmationLevel,
            DateTime? confirmedOn,
            string data,
            DateTime? deletedOn,
            string error,
            string from,
            BigInteger gasAmount,
            BigInteger gasPrice,
            string hash,
            bool includeFee,
            bool isConfirmed,
            BigInteger nonce,
            string signedData,
            TransactionState state,
            string to,
            Guid transactionId)
        {
            Amount = amount;
            BlockNumber = blockNumber;
            BroadcastedOn = broadcastedOn;
            BuiltOn = builtOn;
            CompletedOn = completedOn;
            ConfirmationLevel = confirmationLevel;
            ConfirmedOn = confirmedOn;
            Data = data;
            DeletedOn = deletedOn;
            Error = error;
            From = from;
            GasAmount = gasAmount;
            GasPrice = gasPrice;
            Hash = hash;
            IncludeFee = includeFee;
            IsConfirmed = isConfirmed;
            Nonce = nonce;
            SignedData = signedData;
            State = state;
            To = to;
            TransactionId = transactionId;
        }
        
        public static Transaction Create(
            Guid transactionId,
            string from,
            string to,
            BigInteger amount,
            BigInteger gasAmount,
            BigInteger gasPrice,
            bool includeFee,
            BigInteger nonce,
            string data)
        {
            return new Transaction
            (
                amount: amount,
                builtOn: DateTime.UtcNow,
                data: data,
                from: from,
                gasAmount: gasAmount,
                gasPrice: gasPrice,
                includeFee: includeFee,
                nonce: nonce,
                state: TransactionState.Built,
                to: to,
                transactionId: transactionId
            );
        }
        
        
        public BigInteger Amount { get; }
        
        public BigInteger? BlockNumber { get; private set; }
        
        public DateTime? BroadcastedOn { get; private set; }
        
        public DateTime BuiltOn { get; }
        
        public DateTime? CompletedOn { get; private set; }
        
        public BigInteger? ConfirmationLevel { get; private set; }
        
        public DateTime? ConfirmedOn { get; private set; }
        
        public string Data { get; }
        
        public DateTime? DeletedOn { get; private set; }
        
        public string Error { get; private set; }
        
        public string From { get; }
        
        public BigInteger GasAmount { get; }
        
        public BigInteger GasPrice { get; }
        
        public string Hash { get; private set; }
        
        public bool IncludeFee { get; }
        
        public bool IsConfirmed { get; private set; }
        
        public BigInteger Nonce { get; }
        
        public string SignedData { get; private set; }
        
        public TransactionState State { get; private set; }
        
        public string To { get; }
        
        public Guid TransactionId { get; }
        
        
        public void OnBroadcasted(
            string signedData,
            string hash)
        {
            if (State == TransactionState.Built)
            {
                BroadcastedOn = DateTime.UtcNow;
                Hash = hash;
                SignedData = signedData;
                State = TransactionState.InProgress;
            }
            else
            {
                throw new InvalidOperationException
                (
                    $"Transaction can not be broadcasted from current [{State.ToString()}] state."
                );
            }
        }

        public void OnConfirmed(
            BigInteger confirmationLevel)
        {
            if (State == TransactionState.Completed || State == TransactionState.Failed)
            {
                ConfirmedOn = DateTime.UtcNow;
                ConfirmationLevel = confirmationLevel;
                IsConfirmed = true;
            }
            else
            {
                throw new InvalidOperationException
                (
                    $"Transaction can not be confirmed in current [{State.ToString()}] state."
                );
            }
        }
        
        public void OnDeleted()
        {
            if (State != TransactionState.Deleted)
            {
                DeletedOn = DateTime.UtcNow;
                State = TransactionState.Deleted;
            }
        }

        public void OnFailed(
            BigInteger blockNumber,
            string error)
        {
            if (State == TransactionState.InProgress)
            {
                BlockNumber = blockNumber;
                CompletedOn = DateTime.UtcNow;
                Error = error;
                State = TransactionState.Failed;
            }
            else
            {
                throw new InvalidOperationException
                (
                    $"Transaction can not fail from current [{State.ToString()}] state."
                );
            }
        }

        public void OnSucceeded(
            BigInteger blockNumber)
        {
            if (State == TransactionState.InProgress)
            {
                BlockNumber = blockNumber;
                CompletedOn = DateTime.UtcNow;
                State = TransactionState.Completed;
            }
            else
            {
                throw new InvalidOperationException
                (
                    $"Transaction can not succeed from current [{State.ToString()}] state."
                );
            }
        }
    }
}