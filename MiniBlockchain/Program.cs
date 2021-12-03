using System.Net;
using System.Text.Json;
using MiniBlockchain;
using MiniBlockchain.core;
using MiniBlockchain.socket;

var blockchain = Blockchain.GetBlockchain();
var minerAddress = "coinbase";

new Thread(() =>
{
    Thread.CurrentThread.IsBackground = true;
    var actionsController = new ActionsController();
    var server = new Server(actionsController);
    server.Start();
}).Start();

int option = 0;

Console.WriteLine("Welcome to the blockchain admin panel");
Console.WriteLine(Environment.NewLine + "--------------------------------");
Console.WriteLine("1. Mine block");
Console.WriteLine("2. Display blockchain");
Console.WriteLine("3. Add miner address");
Console.WriteLine("4. Exit");
Console.WriteLine("--------------------------------" + Environment.NewLine);
Console.WriteLine("Please choose an operation....");

while (option != 4)
{
    switch (option)
    {
        case 1:
            var previousBlock = blockchain.GetLastBlock();
            var previousHash = previousBlock.Hash;
            var newBlock = blockchain.CreateBlockWithPendingTransactions(previousHash, minerAddress);

            Console.WriteLine(JsonSerializer.Serialize(newBlock));
            break;
        case 2:
            Console.WriteLine(JsonSerializer.Serialize(blockchain.Chain));
            break;
        case 3:
            Console.WriteLine("Please type the address you want the coins to go to:");
            minerAddress = Console.ReadLine();
            break;
    }
    var input = Console.ReadLine();
    option = int.Parse(input);
}