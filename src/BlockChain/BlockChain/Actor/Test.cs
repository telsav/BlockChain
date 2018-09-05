using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain.Actor
{
    public class Tests
    {
        public void IsValid()
        {
            int i = 0;
            Parallel.ForEach(
                Enumerable.Range(1,int.MaxValue),
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                (line, state, index) =>
                {

                var account = new AccountActor();
                var output = new OutputActor();

                account.Send(new Deposit { Amount = 50 + line });
                account.Send(new QueryBalance
                {
                    Receiver = output
                });

                //account.Completion.Wait();
                //output.Completion.Wait();

                Console.WriteLine($"This is the {++i} actor,");
            });

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
