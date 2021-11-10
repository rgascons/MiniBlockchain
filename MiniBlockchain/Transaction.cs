using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Secp256k1Net;

namespace MiniBlockchain
{
    public class Transaction
    {
        public string? FromAddress { get; set; }
        public string ToAddress { get; set;  }
        public double Amount { get; set; }
        [JsonIgnore]
        public string? Signature { get; internal set; }

        public Transaction() { }

        /// <summary>
        /// Initialize an unsigned transaction object.
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

        /// <summary>
        /// Initialize a signed transaction object.
        /// </summary>
        /// <param name="fromAddress">The senders address.</param>
        /// <param name="toAddress">The receivers address.</param>
        /// <param name="amount">The transation amount.</param>
        /// <param name="signature">The signature.</param>
        public Transaction(string fromAddress, string toAddress, double amount, string signature)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
            Signature = signature;
        }

        /// <summary>
        /// Re-calculates the hash of a transaction.
        /// </summary>
        /// <returns>A hash of the transaction.</returns>
        public byte[] CalculateHash()
        {
            using (SHA256 hash = SHA256.Create())
            {
                var encodedBlock = JsonConvert.SerializeObject(this);
                var byteHash = hash.ComputeHash(Encoding.UTF8.GetBytes(encodedBlock));
                return byteHash;
            }
        }

        public void SignTransaction(Wallet keyPair)
        {
            if (FromAddress != keyPair.PublicKey)
            {
                throw new Exception("You can sign transactions for your wallet only.");
            }
            var hashTx = CalculateHash();
            Signature = keyPair.Sign(hashTx);
        }

        public bool Validate()
        {
            if (FromAddress == null)
            {
                return true;
            }

            if (Signature == null)
            {
                return false;
            }

            using (var secp256k1 = new Secp256k1())
            {
                return secp256k1.Verify(
                    Convert.FromHexString(Signature),
                    System.Security.Cryptography.SHA256.Create().ComputeHash(CalculateHash()),
                    Convert.FromHexString(FromAddress)
                    );

            }
        }
    }
}

