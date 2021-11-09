using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace MiniBlockchain
{
    public record Block
    {
        public Block(long index, string previousHash)
        {
            Index = index;
            Hash = this.calculateHash();
            TimeStamp = DateTime.UtcNow;
            PreviousHash = previousHash;
            Nonce = 0;
        }

        public long Index { get; }

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

        public bool ShouldSerializeHash()
        {
            return false;
        }

        /// <summary>
        /// Re-calculates the hash of a block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <returns>A hash of the block.</returns>
        public string calculateHash()
        {
            using (SHA256 hash = SHA256.Create())
            {
                var encodedBlock = JsonConvert.SerializeObject(this);
                var byteHash = hash.ComputeHash(Encoding.UTF8.GetBytes(encodedBlock));
                return Convert.ToHexString(byteHash);
            }
        }

        public void mineBlock(string difficulty)
        {
            while(!this.Hash.StartsWith(difficulty))
            {
                this.Nonce++;
                this.Hash = calculateHash();
            }
        }
    }
}
