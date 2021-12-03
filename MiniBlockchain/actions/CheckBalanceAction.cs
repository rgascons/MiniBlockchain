using System;
namespace MiniBlockchain.actions
{
    public class CheckBalanceAction : IAction
    {
        public CheckBalanceAction()
        {
        }

        public string Run(string? payload)
        {
            var blockchain = Blockchain.GetBlockchain();
            var balance = blockchain.GetBalanceOfAddress(payload);

            return $"Your balance is: ${balance}";
        }
    }
}

