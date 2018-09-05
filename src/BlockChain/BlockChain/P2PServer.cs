using BlockChain.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockChain
{
    public class P2PServer : WebSocketBehavior
    {
        private string _ipAddress;
        private int _port;
        private BlockChain _blockchain;

        public P2PServer():this (null,0,null){ }

        public P2PServer(string ipAddress, int port, BlockChain blockchain)
        {
            _ipAddress = ipAddress??String.Empty;
            _port = port;
            _blockchain = blockchain??new BlockChain(null,null);
        }

        bool chainSynched = false;
        WebSocketServer wss = null;

        public void Start()
        {
            wss = new WebSocketServer($"ws://{_ipAddress}:{_port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain",_=> new P2PServer(_ipAddress,_port,_blockchain));
            wss.Start();
            Console.WriteLine($"Started server at ws://{_ipAddress}:{_port}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                BlockChain newChain = JsonConvert.DeserializeObject<BlockChain>(e.Data);
                if (newChain != null)
                {
                    Console.WriteLine("Receive the newChain");
                }
                if (_blockchain != null)
                {
                    Console.WriteLine("Receive the _blockchain");
                }
                if (newChain.IsValid() && newChain.Count > _blockchain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(_blockchain.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    _blockchain = newChain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(_blockchain));
                    chainSynched = true;
                }
            }
        }
    }

}
