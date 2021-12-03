using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using ProtoBuf;
using Secp256k1Net;
using Shared.models;

namespace Shared
{
    public static class Utils
    {
        /// <summary>
        /// Re-calculates the hash of a transaction.
        /// </summary>
        /// <returns>A hash of the transaction.</returns>
        public static byte[] CalculateHash(object obj)
        {
            using (SHA256 hash = SHA256.Create())
            {
                var encodedBlock = JsonConvert.SerializeObject(obj);
                var byteHash = hash.ComputeHash(Encoding.UTF8.GetBytes(encodedBlock));
                return byteHash;
            }
        }

        public static bool ValidateTransaction(Transaction transaction)
        {
            if (transaction.FromAddress == null)
            {
                return true;
            }

            if (transaction.Signature == null)
            {
                return false;
            }

            using (var secp256k1 = new Secp256k1())
            {
                return secp256k1.Verify(
                    Convert.FromHexString(transaction.Signature),
                    System.Security.Cryptography.SHA256.Create().ComputeHash(Utils.CalculateHash(transaction)),
                    Convert.FromHexString(transaction.FromAddress)
                    );

            }
        }

        public static T DeserializeProtobuf<T>(string base64)
        {
            byte[] payloadByteArray = Convert.FromBase64String(base64);
            using (var stream = new MemoryStream(payloadByteArray))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public static string SerializeProtobuf<T>(T o)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, o);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}

