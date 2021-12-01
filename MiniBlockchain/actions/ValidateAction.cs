using System;
namespace MiniBlockchain.actions
{
    public class ValidateAction : IAction
    {
        public ValidateAction()
        {
        }

        public string Run()
        {
            var blockchain = Blockchain.GetBlockchain();
            var isValid = blockchain.IsChainValid();

            if (isValid)
            {
                return "The blockchain is valid.";
            }
            return "The blockchain is NOT valid.";
        }
    }
}

