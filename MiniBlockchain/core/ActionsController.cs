using MiniBlockchain.actions;

namespace MiniBlockchain.core
{
    public class ActionsController
    {
        private Dictionary<string, IAction> map;

        public ActionsController()
        {
            map = new Dictionary<string, IAction>();

            map.Add("get_chain", new GetChainAction());
            map.Add("validate", new ValidateAction());
            map.Add("send_transaction", new SendTransactionAction());
        }

        public string RunAction(string action, string payload = null)
        {
            Console.WriteLine("Running {0}", action);
            return map[action].Run(payload);
        }
    }
}

