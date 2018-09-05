using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Actor
{
    public abstract class Message { }

    public class Deposit : Message
    {
        public decimal Amount { get; set; }
    }

    public class QueryBalance : Message
    {
        public Actor Receiver { get; set; }
    }

    public class Balance : Message
    {
        public decimal Amount { get; set; }
    }

    public class AccountActor : Actor
    {
        private decimal _balance;

        public void Handle(Deposit message)
        {
            _balance += message.Amount;
        }

        public void Handle(QueryBalance message)
        {
            message.Receiver.Send(new Balance { Amount = _balance });
        }
    }

    public class OutputActor : Actor
    {
        public void Handle(Balance message)
        {
            Console.WriteLine("Balance is {0}", message.Amount);
        }
    }

}
