using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace MiniBlockchain
{
    public record Block
    {
        [JsonIgnore]
        public string Hash { get; set; }

        /// <summary>
        /// We don't want Hash property to be serialized when mining the block.
        /// But we want to deserialize the Hash when calling get_chain. This allows us to do that.
        /// </summary>
        [JsonProperty]
        public string DeserializeHash
        {
            set { Hash = value; }
        }

        public DateTime TimeStamp { get; }
        public long Nonce { get; set; }
        public string PreviousHash { get; }
        public IList<Transaction> Transactions { get; }

        /// <summary>
        /// Initialized a block.
        /// </summary>
        /// <param name="previousHash">The previous block hash.</param>
        /// <param name="pendingTransactions">List of pending transactions.</param>
        public Block(string previousHash, IList<Transaction> pendingTransactions)
        {
            Hash = this.CalculateHash();
            TimeStamp = DateTime.UtcNow;
            PreviousHash = previousHash;
            Nonce = 0;
            Transactions = pendingTransactions;
        }

        /// <summary>
        /// Re-calculates the hash of a block.
        /// </summary>
        /// <returns>A hash of the block.</returns>
        public string CalculateHash()
        {
            using (SHA256 hash = SHA256.Create())
            {
                var encodedBlock = JsonConvert.SerializeObject(this);
                var byteHash = hash.ComputeHash(Encoding.UTF8.GetBytes(encodedBlock));
                return Convert.ToHexString(byteHash);
            }
        }

        /// <summary>
        /// Mines a block.
        /// </summary>
        /// <param name="difficulty">The mining difficulty.</param>
        public void MineBlock(string difficulty)
        {
            while(!this.Hash.StartsWith(difficulty))
            {
                this.Nonce++;
                this.Hash = CalculateHash();
            }
        }

        /// <summary>
        /// Validates all transactions.
        /// </summary>
        /// <returns>A boolean indicating if transactions are valid or not.</returns>
        public bool AreTransactionsValid() => Transactions.All((t) => t.Validate());
    }
}
