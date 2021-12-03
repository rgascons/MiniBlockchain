using System.Text;
using WalletClient;
using ProtoBuf;
using Shared.models;
using Shared;

void callback(string res)
{
    Console.WriteLine(res);
}

var host = "Ricards-MacBook-Pro.local";
SocketClient client = new SocketClient(host, callback);

int option = 0;
Wallet wallet = null;

Console.WriteLine(Environment.NewLine + "--------------------------------");
Console.WriteLine("1. Display blockchain");
Console.WriteLine("2. Send a transaction");
Console.WriteLine("3. Create wallet");
Console.WriteLine("4. Validate blockchain");
Console.WriteLine("5. Exit");
Console.WriteLine("--------------------------------" + Environment.NewLine);
Console.WriteLine("Please choose an operation....");

while (option != 5)
{
    switch (option)
    {
        case 1:
            client.Start("get_chain");
            break;
        case 2:
            var transaction = new Transaction
            {
                FromAddress = wallet.PublicKey,
                ToAddress = "mike",
                Amount = 10,
            };
            var hash = Utils.CalculateHash(transaction);
            var signature = wallet.Sign(hash);
            transaction.Signature = signature;
            client.Start("send_transaction", Utils.SerializeProtobuf<Transaction>(transaction));
            break;
        case 3:
            if (wallet == null)
            {
                wallet = new Wallet();
            }
            Console.WriteLine($"Public key: {wallet.PublicKey}");
            Console.WriteLine($"Private key: {wallet.PrivateKey}");
            break;
        case 4:
            client.Start("validate");
            break;
    }
    var input = Console.ReadLine();
    option = int.Parse(input);
}

client.DisposeClient();