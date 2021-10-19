using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using WebSocketSharp;

namespace BlockchainCoding
{
    public class P2PServer : WebSocketBehavior
    {
        public bool isServerSynched = false;
        WebSocketServer webSocketServer = null;

        public void Start()
        {
            webSocketServer = new WebSocketServer($"ws://172.18.208.1:{Program.Port}");// your local ip address 
            webSocketServer.AddWebSocketService<P2PServer>("/Blockchain");
            webSocketServer.Start();
            Console.WriteLine($"Sever is started this address:  ws://172.18.208.1:{Program.Port}");
        }


        protected override void OnMessage(MessageEventArgs e)
        {
            
            if (e.Data == "Hello Server")
            {
              
                Console.WriteLine(e.Data);
                Send("Hello Client");
            }
             
            else
            {
                Blockchain newBlockChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                if (newBlockChain.IsValid() && newBlockChain.chain.Count > Program.blockchain.chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newBlockChain.pendingTransactions);
                    newTransactions.AddRange(Program.blockchain.pendingTransactions);
                    newBlockChain.pendingTransactions = newTransactions;
                    Program.blockchain = newBlockChain;

                }
            }

            if (!isServerSynched)
            {
                Send(JsonConvert.SerializeObject(Program.blockchain));
                isServerSynched = true;
            }

        }

    }
}
