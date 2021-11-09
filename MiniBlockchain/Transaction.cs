using System;
namespace MiniBlockchain
{
    public class Transaction
    {
        public string? FromAddress { get; }
        public string ToAddress { get; }
        public double Amount { get; }

        /// <summary>
        /// Initialized a transaction object.
        /// </summary>
        /// <param name="fromAddress">The senders address.</param>
        /// <param name="toAddress">The receivers address.</param>
        /// <param name="amount">The transation amount.</param>
        public Transaction(string? fromAddress, string toAddress, double amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
        }
    }
}

