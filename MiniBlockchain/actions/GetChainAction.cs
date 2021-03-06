using System;
using System.Text.Json;

namespace MiniBlockchain.actions
{
    public class GetChainAction : IAction
    {
        public GetChainAction()
        {
        }

        public string Run(string? payload)
        {
            var blockchain = Blockchain.GetBlockchain();
            return JsonSerializer.Serialize(blockchain.Chain);
        }
    }
}

