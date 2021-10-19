using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlockchainCoding
{
    
    class Program
    {
        public static Blockchain blockchain = new Blockchain();
        public static int Port = 0;
        public static int ConstPort = 5001;
        public static P2PClient client = new P2PClient();
        public static P2PServer server = null;
        public static string name = "unknown";
       

        static void Main(string[] args)
        {
            string serverUrl = "";
            var dateTime = DateTime.Now;
            int choice = 0;
            Console.WriteLine("Enter Port: ");
            string your_port = Console.ReadLine();

            Console.WriteLine("Enter Name: ");
            string your_name = Console.ReadLine();


            blockchain.InitializeBlock();
            if(your_port.Length >= 1)
             Port = int.Parse(your_port); // for port value
            if (your_name.Length >= 2)
                name = your_name;
            if(Port > 0)
            {
                server = new P2PServer();
                server.Start();
            }

            if (name != "unknown")
                Console.WriteLine("Now User is: "+ name);

            
            Console.WriteLine("//////==//////==//////");
            Console.WriteLine("1. URL Connected");
            Console.WriteLine("2. Add Transaction");
            Console.WriteLine("3. Show the Blockchain");
            Console.WriteLine("4. Show the All Balance in Chain");
            Console.WriteLine("5. Change Sender Name");
            Console.WriteLine("6. Get Servers");
            Console.WriteLine("7. Control is Valid");
            Console.WriteLine("8. Get All Clients Amount");
            Console.WriteLine("9. Exit");
              Console.WriteLine("//////==//////==//////");
            
            while (choice != 10)
            {
                switch (choice)
                {
                    case 1:
                        if (Port == 5001)
                            serverUrl = $"ws://172.18.208.1:{Port + 1}";
                        else if (Port == 5002)
                            serverUrl = $"ws://172.18.208.1:{Port - 1}";
                        else
                            serverUrl = $"ws://172.18.208.1:{ConstPort}";
                        Console.WriteLine("Connected URL server:)");
                        client.Connect($"{serverUrl}/Blockchain");
                        break;

                    case 2:
                        Console.WriteLine("Please enter recevier name: ");
                        string recevierName = Console.ReadLine();
                        Console.WriteLine("Please enter Amount: ");
                        string amount = Console.ReadLine();
                        blockchain.CreateTransaction(new Transaction(name,recevierName,int.Parse(amount))); 
                        blockchain.ProcessPendingTransaction(name);
                        client.BroadCast(JsonConvert.SerializeObject(blockchain));
                        break;

                    case 3:
                        Console.WriteLine(JsonConvert.SerializeObject(blockchain,Formatting.Indented));
                        break;
                    case 4:
                        Console.WriteLine("All Money in Chain: " + blockchain.GetAllBalanceAmount());
                        break;
                    case 5:
                        Console.WriteLine("Enter Sender Name: ");
                        name = Console.ReadLine();
                        break;
                    case 6:
                        for(int i=0; i<client.GetServers().Count;i++)
                          Console.WriteLine(client.GetServers()[i]);
                        break;
                    case 7:
                        Console.WriteLine("Is Valid:  "+blockchain.IsValid());
                        break;
                    case 8:
                        for(int i=0; i < blockchain.chain.Count; i++)
                        {
                            for(int j=0; j < blockchain.chain[i].Transactions.Count; j++)
                            {
                                Console.WriteLine(blockchain.chain[i].Transactions[j].ToAddress + ": " + blockchain.chain[i].Transactions[j].Amount);
                            }
                        }
                        break;

                    


                }


                Console.WriteLine("Please enter a choice: ");
                string action = Console.ReadLine();
                int.TryParse(action,out choice);
            }

            client.Close();






            //string[] clients = { "Mahmut", "Mücahit", "Elon", "Bill" };


            //for (int i = 0; i < clients.Length; i++)
            //{
            //    if (i != clients.Length - 1)
            //    {
            //        blockchain.CreateTransaction(new Transaction(clients[i], clients[i + 1], (i * 2) + 10));
            //        blockchain.CreateTransaction(new Transaction(clients[i + 1], clients[i], i + 20));

            //    }
            //    blockchain.ProcessPendingTransaction("Jeff"); // If you see the balance you need to be write this method on GetBalanceAmount() Method
            //    Console.WriteLine(clients[i] + " has amount: " + blockchain.GetBalanceAmount(clients[i]));

            //}

            //var dateTime2 = DateTime.Now;
            //Console.WriteLine("Finish Time: " + (dateTime - dateTime2).ToString());
            //Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));
            //Console.WriteLine("Is Valid: " + blockchain.IsValid());
            //Console.ReadKey();


            /*
         for(int i=0; i < clients.Length; i++)
         {
             if(i != clients.Length - 1)
             {
                  blockchain.AddBlock(new Block(DateTime.Now, null, "Sender:" + clients[i] + " reciver:" + clients[i + 1] + " Amount: "+ (i+20)));

             }
         }
         */

            //Random random = new Random();
            //Blockchain blockchain = new Blockchain();
            //List<int> players = new List<int>(); 
            //List<int> playerAmount = new List<int>();
            //int maxPlayer = 50;
            //for(int i=0; i < maxPlayer; i++)
            //{
            //    players.Add(i + 1);
            //    playerAmount.Add((i * 2) + 100);
            //}

            //while(players.Count != 1)
            //{

            //    int chooseDeadNumber = random.Next(players.Count);
            //    string deadName = players[chooseDeadNumber].ToString();
            //    players.RemoveAt(chooseDeadNumber);
            //    blockchain.CreateTransaction(new Transaction(deadName, players[0].ToString(), playerAmount[chooseDeadNumber]));
            //    playerAmount.RemoveAt(chooseDeadNumber);
            //blockchain.ProcessPendingTransaction("Jeffrey");

            //}
            //Console.WriteLine("Winner is "+players[0].ToString()+" Reward: " +blockchain.GetBalanceAmount(players[0].ToString()));
            //Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

        }
    }
}
