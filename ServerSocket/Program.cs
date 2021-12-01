// See https://aka.ms/new-console-template for more information
using MiniBlockchain.socket;

Console.WriteLine("Hello, World!");
new Thread(() =>
{
    Thread.CurrentThread.IsBackground = true;
    var server = new Server();
    server.Start();
}).Start();

Console.WriteLine("\nPress ENTER to exit...");
Console.Read();