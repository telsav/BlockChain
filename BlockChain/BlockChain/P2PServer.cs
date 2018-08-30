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
        private Blockchain _blockchain;

        public P2PServer(){ }

        public P2PServer(string ipAddress, int port, Blockchain blockchain)
        {
            _ipAddress = ipAddress;
            _port = port;
            _blockchain = blockchain;
        }

        bool chainSynched = false;
        WebSocketServer wss = null;

        public void Start()
        {
            wss = new WebSocketServer($"ws://{_ipAddress}:{_port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
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
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (newChain.IsValid() && newChain.Chain.Count > _blockchain.Chain.Count)
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
