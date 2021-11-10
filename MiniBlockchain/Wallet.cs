using System;
using System.Security.Cryptography;
using Secp256k1Net;

namespace MiniBlockchain
{
    public class Wallet
    {
        public string PrivateKey { internal set; get; }
        public string PublicKey { internal set; get; }

        public Wallet()
        {
            GenerateKeyPair();
        }

        public Wallet(string privateKey, string publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        private void GenerateKeyPair()
        {
            using (var secp256k1 = new Secp256k1())
            {

                // Generate a private key.
                var privateKey = new byte[32];
                var rnd = System.Security.Cryptography.RandomNumberGenerator.Create();
                do { rnd.GetBytes(privateKey); }
                while (!secp256k1.SecretKeyVerify(privateKey));


                // Create public key from private key.
                var publicKey = new byte[64];
                secp256k1.PublicKeyCreate(publicKey, privateKey);

                PrivateKey = Convert.ToHexString(privateKey);
                PublicKey = Convert.ToHexString(publicKey);
            }
        }

        public string Sign(byte[] message)
        {
            using (var secp256k1 = new Secp256k1())
            {
                var messageHash = System.Security.Cryptography.SHA256.Create().ComputeHash(message);
                var signature = new byte[64];
                var isValid = secp256k1.Sign(signature, messageHash, Convert.FromHexString(PrivateKey));
                if (!isValid) throw new Exception("Wrong private-pubic key pair");
                return Convert.ToHexString(signature);
            }
        }
    }
}

