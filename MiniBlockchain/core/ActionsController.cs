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
        }

        public string RunAction(string action)
        {
            Console.WriteLine("Running {0}", action);
            return map[action].Run();
        }
    }
}

