using System;
using Shared;
using Shared.models;

namespace MiniBlockchain
{
    public class TransactionPool
    {
        private List<Transaction> _pendingTransactions;
        public IList<Transaction> PendingTransactions {
            get => _pendingTransactions.AsReadOnly();
        }

        public TransactionPool()
        {
            _pendingTransactions = new List<Transaction>();
        }

        /// <summary>
        /// Adds a transaction to the list of pending transactions.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public void AddTransaction(Transaction transaction)
        {
            if (transaction.FromAddress == null || transaction.ToAddress == null)
            {
                throw new Exception("Transaction needs a sender and receiver address.");
            }

            if (!Utils.ValidateTransaction(transaction))
            {
                throw new Exception("Transaction is not properly signed");
            }
            _pendingTransactions.Add(transaction);
        }

        public void ClearPendingTransactions(Transaction initialTx)
        {
            _pendingTransactions = new List<Transaction>() { initialTx };
        }
    }
}

