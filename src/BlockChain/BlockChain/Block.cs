using System;
using System.Collections.Generic;

namespace BlockChain
{
    public class Block:IBlock
    {

        public byte[] Data { get; }

        public byte[] Hash { get; set; }

        public byte[] PreviousHash { get; set; }

        public int Nonce { get; set; }

        public DateTime TimeStamp { get; }

        public IList<Transaction> Transactions { get; set; }

        public Block(byte[] data,IList<Transaction> transactions)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Nonce = 0;
            PreviousHash = new byte[] { 0x00 };
            TimeStamp = DateTime.Now;
            Transactions = transactions;
        }


        public override string ToString()
        {
            return String.Format("{0}:\n:{1}:\n:{2}:\n{3}\n\r", BitConverter.ToString(Hash).Replace("-", ""), BitConverter.ToString(PreviousHash).Replace("-", ""), Nonce, TimeStamp);
        }

    }

}
