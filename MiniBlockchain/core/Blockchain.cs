using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MiniBlockchain
{
    public class Blockchain
    {

        /// <summary>
        /// The blockchain.
        /// </summary>
        public List<Block> Chain { get; }

        /// <summary>
        /// Current difficulty of proof of work.
        /// </summary>
        private readonly string difficulty;

        /// <summary>
        /// List of pending (unverified) transactions.
        /// </summary>
        private TransactionPool txPool;

        /// <summary>
        /// Mining reward when a block is mined.
        /// </summary>
        private readonly long miningReward;

        private static Blockchain BlockchainInstance { get; set; }

        /// <summary>
        /// Initializes a blockchain with one block.
        /// </summary>
        private Blockchain()
        {
            Chain = new List<Block>()
            {
                // Genesis block
                new Block("0", new List<Transaction>())
            };
            difficulty = "000";
            txPool = new TransactionPool();
            miningReward = 10;
        }

        public static Blockchain GetBlockchain()
        {
            if (BlockchainInstance == null)
            {
                BlockchainInstance = new Blockchain();
            }
            return BlockchainInstance;
        }

        /// <summary>
        /// Create a new block and add it to the blockchain.
        /// </summary>
        /// <param name="previousHash">The previous block hash.</param>
        /// <returns>Newly created block.</returns>
        public Block CreateBlockWithPendingTransactions(string previousHash, string minnerAddress)
        {
            var block = new Block(previousHash, txPool.PendingTransactions);
            block.MineBlock(difficulty);
            Chain.Add(block);
            txPool.ClearPendingTransactions(new Transaction(null, minnerAddress, miningReward));
            return block;
        }

        /// <summary>
        /// Adds a transaction to the list of pending transactions.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public void AddTransaction(Transaction transaction)
        {
            txPool.AddTransaction(transaction);
        }

        /// <summary>
        /// Gets the balance of a given address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The balance.</returns>
        public double GetBalanceOfAddress(string address)
        {
            double balance = 0;

            Chain.ForEach((block) =>
            {
                foreach(var transaction in block.Transactions)
                {
                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }
                    else if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            });

            return balance;
        }

        /// <summary>
        /// Print the last block of the blockchain.
        /// </summary>
        /// <returns>Block</returns>
        public Block GetLastBlock()
        {
            return Chain[Chain.Count - 1];
        }

        /// <summary>
        /// Validates the chain.
        /// </summary>
        /// <returns>boolean</returns>
        public bool IsChainValid()
        {
            var previousBlock = this.Chain[0];
            var blockIndex = 1;
            while (blockIndex < this.Chain.Count)
            {
                var block = this.Chain[blockIndex];
                if (block.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }

                if (!block.AreTransactionsValid())
                {
                    return false;
                }

                var calculatedHash = block.CalculateHash();
                if (block.Hash != calculatedHash)
                {
                    return false;
                }

                previousBlock = block;
                blockIndex++;
            }
            return true;
        }
    }
}
