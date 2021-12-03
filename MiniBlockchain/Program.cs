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
/*
app.UseHttpsRedirection();

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

app.Run();
*/
Console.ReadLine();