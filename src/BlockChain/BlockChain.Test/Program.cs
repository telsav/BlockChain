using BlockChain.Actor;
using BlockChain.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace BlockChain.Test
{
    class Program
    {
        public static string ipAddress="127.0.0.1";
        public static int Port = 0;
        public static P2PServer Server = null;
        public static P2PClient Client = null;
        private static Random random = new Random(DateTime.Now.Millisecond);
        private static readonly IBlock genesis = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, null);
        /** proof of work (https://en.wikipedia.org/wiki/Proof-of-work_system)  is set here: a hash needs 2 trailing zero bytes, increase the number of bytes to reduce the number of valid  hashes, and increse the proof of work time **/
        private static readonly byte[] difficulty = new byte[] { 0x00, 0x00 };
        public static BlockChain PhillyCoin = new BlockChain(difficulty,genesis);
        public static string name = "Unknown";

        static void Main(string[] args)
        {

            new Tests().IsValid();

            //TestBlockChain(args);
        }


        static void TestBlockChain(string[] args)
        {
            if (args.Length >= 1)
                Port = int.Parse(args[0]);
            if (args.Length >= 2)
                name = args[1];

            if (Port > 0)
            {
                Server = new P2PServer(ipAddress, Port, PhillyCoin);
                Server.Start();
                Console.WriteLine($"P2P server is running on {ipAddress}:{Port}......");
            }
            if (name != "Unkown")
            {
                Console.WriteLine($"Current user is {name}");
            }

            Console.WriteLine("=========================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Display Blockchain");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL");
                        string serverURL = Console.ReadLine();
                        Client = new P2PClient(PhillyCoin);
                        Client.Connect($"{serverURL}/Blockchain");
                        break;
                    case 2:
                        Console.WriteLine("Please enter the receiver name");
                        string receiverName = Console.ReadLine();
                        Console.WriteLine("Please enter the amount");
                        string amount = Console.ReadLine();
                        PhillyCoin.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount)));
                        PhillyCoin.ProcessPendingTransactions(name);
                        Client = new P2PClient(PhillyCoin);
                        Client.Broadcast(JsonConvert.SerializeObject(PhillyCoin));
                        break;
                    case 3:
                        Console.WriteLine("BlockChain");
                        Console.WriteLine(JsonConvert.SerializeObject(PhillyCoin, Formatting.Indented));
                        break;

                }

                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }

            Client.Close();
        }
    }
}
