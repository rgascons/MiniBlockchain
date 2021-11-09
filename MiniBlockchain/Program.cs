using MiniBlockchain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var blockchain = new Blockchain();

app.MapGet("/mine_block", () =>
{
    var previousBlock = blockchain.printLastBlock();

    var previousHash = previousBlock.Hash;
    var newBlock = blockchain.createBlock(previousHash);

    return newBlock;
});

app.MapGet("/get_chain", () =>
{
    return blockchain.chain;
});

app.MapGet("/validate", () =>
{
    var isValid = blockchain.isChainValid();
    
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