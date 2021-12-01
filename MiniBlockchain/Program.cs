using System.Net;
using MiniBlockchain;
using MiniBlockchain.core;
using MiniBlockchain.socket;

var blockchain = Blockchain.GetBlockchain();

new Thread(() =>
{
    Thread.CurrentThread.IsBackground = true;
    var actionsController = new ActionsController();
    var server = new Server(actionsController);
    server.Start();
}).Start();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapPost("/send_transaction", async (context) =>
{
    if (!context.Request.HasJsonContentType())
    {
        context.Response.StatusCode = (int) HttpStatusCode.UnsupportedMediaType;
        return;
    }

    var transaction = await context.Request.ReadFromJsonAsync<Transaction>();

    // This is for dev purposes only. Get the public-private key pair by calling /create_wallet
    //var publicKey = "public-key";
    //var privateKey = "private-key";
    //Wallet wallet = new Wallet(privateKey, publicKey);
    //transaction.SignTransaction(wallet);

    if (transaction != null)
    {
        blockchain.AddTransaction(transaction);
        await context.Response.WriteAsync("Transaction pending confirmation");
    }

    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
});

app.MapGet("/check_balance/{address}", async (context) =>
{
    if (!context.Request.RouteValues.TryGetValue("address", out var address))
    {
        context.Response.StatusCode = 400;
        return;
    }

    var balance = blockchain.GetBalanceOfAddress(address.ToString());

    await context.Response.WriteAsync($"Your balance is: ${balance}");
});

app.MapGet("/mine_block/{miner_address}", async (context) =>
{
    if (!context.Request.RouteValues.TryGetValue("miner_address", out var minerAddress))
    {
        context.Response.StatusCode = 400;
        return;
    }

    var previousBlock = blockchain.GetLastBlock();

    var previousHash = previousBlock.Hash;
    var newBlock = blockchain.CreateBlockWithPendingTransactions(previousHash, minerAddress.ToString());

    await context.Response.WriteAsJsonAsync(newBlock);
});

// Wallet "client"
app.MapGet("/create_wallet", () =>
{
    var keyPair = new Wallet();

    return keyPair;
});

app.Run();