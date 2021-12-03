using System;
using System.Text;
using ProtoBuf;
using Shared;
using Shared.models;

namespace MiniBlockchain.actions
{
    public class SendTransactionAction : IAction
    {
        public SendTransactionAction()
        {
        }

        public string Run(string? payload)
        {
            if (payload == null || payload == "")
            {
                throw new Exception();
            }
            var blockchain = Blockchain.GetBlockchain();
            byte[] payloadByteArray = Convert.FromBase64String(payload);
            var transaction = Utils.DeserializeProtobuf<Transaction>(payload);

            if (transaction != null)
            {
                blockchain.AddTransaction(transaction);
                return "Transaction pending confirmation";
            }
            return "Transaction bad format";
        }
    }
}
