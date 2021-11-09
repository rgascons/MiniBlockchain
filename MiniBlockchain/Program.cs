using MiniBlockchain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var blockchain = new Blockchain();

app.MapGet("send_transaction/{from_address}/{to_address}/{amount}", async (context) =>
{
    if (!context.Request.RouteValues.TryGetValue("from_address", out var fromAddress))
    {
        context.Response.StatusCode = 400;
        return;
    }

    if (!context.Request.RouteValues.TryGetValue("to_address", out var toAddress))
    {
        context.Response.StatusCode = 400;
        return;
    }

    if (!context.Request.RouteValues.TryGetValue("amount", out var amount))
    {
        context.Response.StatusCode = 400;
        return;
    }

    var transaction = new Transaction(fromAddress.ToString(), toAddress.ToString(), double.Parse(amount.ToString()));
    blockchain.CreateTransaction(transaction);

    await context.Response.WriteAsync("Transaction pending confirmation");
});

app.MapGet("check_balance/{address}", async (context) =>
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

app.MapGet("/get_chain", () =>
{
    return blockchain.Chain;
});

app.MapGet("/validate", () =>
{
    var isValid = blockchain.IsChainValid();
    
    if (isValid)
    {
        return "The blockchain is valid.";
    }
    else
    {
        return "The blockchain is NOT valid.";
    }
});

app.Run();