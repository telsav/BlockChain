using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockChain.Extensions
{
    public static class BlockChainExtensions
    {
        public static bool IsValid(this IEnumerable<IBlock> items)
        {
            return items
                .Zip(items.Skip(1), Tuple.Create)
                .All(block => block.Item2.HasValidHash() && block.Item2.HasValidPreviousHash(block.Item1));
        }

        public static void CreateTransaction(this BlockChain blocks, Transaction transaction)
        {
            blocks.PendingTransactions.Add(transaction);
        }

        public static void ProcessPendingTransactions(this BlockChain blocks, string minerAddress)
        {
            IBlock block = new Block(blocks.LastOrDefault().Hash, blocks.PendingTransactions);
            blocks.Add(block);

            blocks.PendingTransactions = new List<Transaction>();
            blocks.CreateTransaction(new Transaction(null, minerAddress,blocks.Reward));
        }


        public static int GetBalance(this BlockChain blocks, string address)
        {
            int balance = 0;

            for (int i = 0; i < blocks.Count; i++)
            {
                for (int j = 0; j < blocks[i].Transactions.Count; j++)
                {
                    var transaction = blocks[i].Transactions[j];

                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }

    }
}
