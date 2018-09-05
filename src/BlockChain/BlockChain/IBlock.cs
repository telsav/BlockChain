using System;
using System.Collections.Generic;

namespace BlockChain
{
    public interface IBlock
    {
        byte[] Data { get; }
        byte[] Hash { get; set; }
        int Nonce { get; set; }
        byte[] PreviousHash { get; set; }
        DateTime TimeStamp { get; }
        IList<Transaction> Transactions { get; set; }
    }
}
