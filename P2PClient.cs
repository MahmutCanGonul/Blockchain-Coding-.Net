using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using Newtonsoft.Json;

namespace BlockchainCoding
{
    public class P2PClient
    {
       public IDictionary<string, WebSocket> webDic = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            if (!webDic.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);
                webSocket.OnMessage += (sender, e) =>
                {
                    if(e.Data == "Hello Client")
                    {
                        Console.WriteLine(e.Data);
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
                };

                webSocket.Connect();
                webSocket.Send("Hello Server");
                webSocket.Send(JsonConvert.SerializeObject(Program.blockchain));
                webDic.Add(url,webSocket);
            }
        }

        public void Send(string url,string data)
        {
            foreach(var item in webDic)
            {
               if(item.Key== url)
                {
                    item.Value.Send(data);
                }
            }
        }


        public void BroadCast(string data)
        {
            foreach(var item in webDic)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach(var item in webDic)
            {
                servers.Add(item.Key);
            }
         
            return servers;

        }

        public void Close()
        {
            foreach(var item in webDic)
            {
                item.Value.Close();
               
            }
        }


    }
}
