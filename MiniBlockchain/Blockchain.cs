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
        public List<Block> chain { get; }

        /// <summary>
        /// Current difficulty of proof of work
        /// </summary>
        private string difficulty;

        private List<Transaction> pendingTransactions;

        private long miningReward;

        /// <summary>
        /// Initializes a blockchain with one block.
        /// </summary>
        public Blockchain()
        {
            chain = new List<Block>();
            difficulty = "000";
            pendingTransactions = new List<Transaction>();
            miningReward = 10;

            // Genesis block
            createBlockWithPendingTransactions("0", "genesis");
        }

        /// <summary>
        /// Create a new block and add it to the blockchain.
        /// </summary>
        /// <param name="previousHash">The previous' block hash.</param>
        /// <returns>Newly created block</returns>
        public Block createBlockWithPendingTransactions(string previousHash, string minnerAddress)
        {
            var block = new Block(previousHash, pendingTransactions);
            block.mineBlock(difficulty);
            chain.Add(block);
            pendingTransactions = new List<Transaction>(){ new Transaction(null, minnerAddress, miningReward) };
            return block;
        }

        public void createTransaction(Transaction transaction)
        {
            pendingTransactions.Add(transaction);
        }

        public double getBalanceOfAddress(string address)
        {
            double balance = 0;

            chain.ForEach((block) =>
            {
                block.Transactions.ForEach((transaction) =>
                {
                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }
                    else if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                });
            });

            return balance;
        }

        /// <summary>
        /// Print the last block of the blockchain.
        /// </summary>
        /// <returns>Block</returns>
        public Block printLastBlock()
        {
            return chain[chain.Count - 1];
        }

        /// <summary>
        /// Validates the chain
        /// </summary>
        /// <returns>boolean</returns>
        public bool isChainValid()
        {
            var previousBlock = this.chain[0];
            var blockIndex = 1;
            while (blockIndex < this.chain.Count)
            {
                var block = this.chain[blockIndex];
                if (block.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }

                var calculatedHash = block.calculateHash();
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
