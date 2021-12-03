using System;
namespace MiniBlockchain.actions
{
    public interface IAction
    {
        string Run(string? payload);
    }
}

