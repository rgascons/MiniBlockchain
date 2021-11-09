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
        public IList<Block> chain { get; }

        /// <summary>
        /// Current difficulty of proof of work
        /// </summary>
        private string difficulty;

        /// <summary>
        /// Initializes a blockchain with one block.
        /// </summary>
        public Blockchain()
        {
            chain = new List<Block>();
            difficulty = "00000";
            // Genesis block
            createBlock("0");
        }

        /// <summary>
        /// Create a new block and add it to the blockchain.
        /// </summary>
        /// <param name="previousHash">The previous' block hash.</param>
        /// <returns>Newly created block</returns>
        public Block createBlock(string previousHash)
        {
            var block = new Block(chain.Count, previousHash);
            block.mineBlock(difficulty);
            chain.Add(block);
            return block;
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
