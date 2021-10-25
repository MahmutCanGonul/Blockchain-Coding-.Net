# Blockchain Coding .Net [STEP BY STEP]
 Create your own Blockchain and do that P2P communication 


➡️1.STEP=> Create a Block:

![image](https://user-images.githubusercontent.com/75094927/138609385-ab3eb005-ce01-4d23-a8e1-10410b15573f.png)

💣This is Block objects:

        public int Index { get; set; }
        public DateTime TimeSpan { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions;
        //public string Data { get; set; }
        public int Nonce { get; set; } = 0;


💣Create Constructer:
     
     public Block(DateTime timeSpan,string previousHash, IList<Transaction> transactions)
        {
            TimeSpan = timeSpan;
            PreviousHash = previousHash;
            //Data = data;
            Transactions = transactions;
            //Hash = CalculateHash();
          
        }
     

💣Calculate Hash: [This is give block a security]
               
              {using  System.Security.Cryptography} => this package provide SHA256
          public string CalculateHash()
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] inBytes = Encoding.ASCII.GetBytes($"{TimeSpan}--{PreviousHash ?? ""}--{JsonConvert.SerializeObject(Transactions)}--{Nonce}"); // We take block datas in byte array 
            byte[] outBytes = sHA256.ComputeHash(inBytes); // Calculate has with SHA256 on our in-bytes
            return Convert.ToBase64String(outBytes); // Change byte array on string
        }


💣Proof of Work Logic: 
 
        public void Mine(int difficulty) // Proof of Work Logic
        {
            var leadingzeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0,difficulty) != leadingzeros)
            {
                this.Nonce++;
                this.Hash = CalculateHash();
            }
        }

🥇Done You are create a block now let's move Transaction Part!!!!



➡️2.STEP=> Create a Transaction Part:




![image](https://user-images.githubusercontent.com/75094927/138609613-472b06b5-f15f-4508-97ae-1a44966ba24e.png)


💣Create Transaction Objects:
     
        public string FromAddress { get; set; } // Sender Address
        public string ToAddress { get; set; } // Reciever Address
        public int Amount { get; set; } // Money :)
        
        
💣Create Transaction Constructer:

       public Transaction(string fromAddress,string toAddress,int amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;

        }
        

🥇Well Done! You are create a block and Transaction Logics! Now let's move Blockchain Part!!!!        


➡️3.STEP=> Create a Blockchain Part:

![image](https://user-images.githubusercontent.com/75094927/138610063-d916a7ae-e2bd-442e-aa44-c96fb3f6624e.png)



💣Create a Objects of Blockchain Part:

        public IList<Transaction> pendingTransactions = new List<Transaction>(); // Pending Transactions are kept
        public IList<Block> chain; // This is our chain
        public int difficulty { get; set; } = 2; // mining difficulty if your coin is valuable difficulty must be increase so that mining will be hard all computers
        public int Reward { get; set; } = 1; // this is mining reward like if you find a coin get a 1 coin



💣Let's start the chain:

           public void InitializeBlock() // Start Chain
        {
            chain = new List<Block>();
            AddGenesisBlock();
        }

💣Create a chain:

        public Block CreateGenesisBlock() //Creat Chain
        {
            Block block = new Block(DateTime.Now, null, pendingTransactions);
            block.Mine(this.difficulty);
            pendingTransactions = new List<Transaction>();
            return block;
        }
        
💣Add chain:
        
         public void AddGenesisBlock() // Add Chain
        {
            chain.Add(CreateGenesisBlock());
        }
        
💣Get Last Chain:

         public Block GetLastBlock() // Get Last Chain
        {
            return chain[chain.Count - 1];
        }

💣Add Block on chain:


         public void AddBlock(Block block)
        {
            Block latestBlock = GetLastBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            block.Mine(this.difficulty); // if I use chain count mining time will be increase and dependent on chain count 
            chain.Add(block);

        }
        
💣Create a Transaction:

         
        public void CreateTransaction(Transaction transaction) // We are create Transaction Here
        {
            pendingTransactions.Add(transaction);

        }
        
        
        
💣Giving Reward for miner:

           
           
           public void ProcessPendingTransaction(string minerAddress)
        {
            CreateTransaction(new Transaction(null, minerAddress, Reward)); // giving miner reward
            Block block = new Block(DateTime.Now,GetLastBlock().Hash,pendingTransactions); // we are write transaction on block
            AddBlock(block); // and we add that on block
            pendingTransactions = new List<Transaction>(); // last part is empty the pending transactions
            }
         
         
 💣Control is valid our blockhain like if someone attack our block isValid will be turn on false:
 
           public bool IsValid()
        {
            bool isValid = true;
            for(int i=1; i < chain.Count; i++)
            {
                Block currentBlock = chain[i];
                Block previousBlock = chain[i - 1];

                if(currentBlock.Hash != currentBlock.CalculateHash())
                {
                    isValid = false;
                }

                if(currentBlock.PreviousHash != previousBlock.Hash)
                {
                    isValid = false;
                }
            }

            return isValid;

        }
        
        
 💣Get Balance from to address:
 
                 public int GetBalanceAmount(string address) // Get Balance from to address
        {
            int balance = 0;
            bool isIn = false;
           for(int i=0; i < chain.Count; i++)
            {
                for(int j=0; j < chain[i].Transactions.Count; j++)
                {
                    var transaction = chain[i].Transactions[j];
                    if(transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                        isIn = true;
                    }

                    if(transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                        isIn = true;
                    }

                }
            }
            if (!isIn)
            {
                Console.WriteLine("I can not find this address in the chain!");
            }

            return balance;

        }
        
        
💣Get Balance all Transactions in blocks:


            public int GetAllBalanceAmount() // Get Balance all Transactions in blocks
        {
            int balance = 0;
            for(int i=0; i < chain.Count; i++)
            {
                for(int j=0; j < chain[i].Transactions.Count; j++)
                {
                    balance += chain[i].Transactions[j].Amount;
                }
            }

            return balance;

        }
        
        

 🥇Well Done! You are create a first blockchain!!!       
       
  
↪️Now we have last goals for P2P communication:

➡️4.STEP=> Create a server for it is help to connection between two or more program:

 🎱We need this package: 
                  
                   using WebSocketSharp.Server; // This package important for server
                   using Newtonsoft.Json; // This package important for send and get json data to each other
                   using WebSocketSharp; // This package important for server
                   
                   
                   
                   
💣Create Objects for P2P Server:
              
        public bool isServerSynched = false;
        WebSocketServer webSocketServer = null;

💣Create a Start Method: 

         public void Start()
        {
            webSocketServer = new WebSocketServer($"ws://YOUR_API_ADDRESS:{Program.Port}");// your local ip address 
            webSocketServer.AddWebSocketService<P2PServer>("/Blockchain");
            webSocketServer.Start();
            Console.WriteLine($"Sever is started this address:  ws://YOUR_API_ADDRESS {Program.Port}");
        }
                   

🛰️By the way how to learn you IP Adress:
           
  💣Open terminal and write [IPv4 is your IP Address]: 
               
                 ipconfig
          
          
 ![image](https://user-images.githubusercontent.com/75094927/138742664-cbf008b3-aa78-456c-ad9f-b86480bbba77.png)

          
          
💣We need to send message on other client so that we need to OnMessage() Method:

         
          protected override void OnMessage(MessageEventArgs e)
        {
            
            if (e.Data == "Hello Server") // If this message come our server from the client 
            {
              
                Console.WriteLine(e.Data); 
                Send("Hello Client"); // We should  send message to Client
            }
             
            else
            {
                Blockchain newBlockChain = JsonConvert.DeserializeObject<Blockchain>(e.Data); // Create a new Blockchain and this blockchain has client data
                
                if (newBlockChain.IsValid() && newBlockChain.chain.Count > Program.blockchain.chain.Count) // if your Blockchain is valid and server blockchain is bigger than our current blockchain do that this things
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newBlockChain.pendingTransactions);
                    newTransactions.AddRange(Program.blockchain.pendingTransactions);
                    newBlockChain.pendingTransactions = newTransactions;
                    Program.blockchain = newBlockChain;

                }
            }

            if (!isServerSynched)  // if isServerSynched is not valid send the our current blockchain like a string format and turn on true isServerSynched 
            {
                Send(JsonConvert.SerializeObject(Program.blockchain));
                isServerSynched = true;
            }

        }
 
  
🥇Wonderfull! You are create a P2P Server you almost done!!!


✈️We have last step:


➡️5.STEP=> Create a client for communicate with server:

💣We need to create objects:
           
          public IDictionary<string, WebSocket> webDic = new Dictionary<string, WebSocket>();

          
💣We need to create Connect Method:

         public void Connect(string url)
        {
            if (!webDic.ContainsKey(url)) // If this url has not on our webDic list
            {
                WebSocket webSocket = new WebSocket(url); // Create a new websocket and it has this url
                webSocket.OnMessage += (sender, e) =>
                {
                    if(e.Data == "Hello Client") // If client recieve a this message
                    {
                        Console.WriteLine(e.Data); // Write that
                    }
                    else If client has not recieve a this message
                    {
                        Blockchain newBlockChain = JsonConvert.DeserializeObject<Blockchain>(e.Data); //Create a blockchain and it has this data
                        if (newBlockChain.IsValid() && newBlockChain.chain.Count > Program.blockchain.chain.Count)
                        {
                            List<Transaction> newTransactions = new List<Transaction>(); // Create a transaction list
                            newTransactions.AddRange(newBlockChain.pendingTransactions); //add late new transaction on new transactions
                            newTransactions.AddRange(Program.blockchain.pendingTransactions);//add late current transaction on new transactions
                            newBlockChain.pendingTransactions = newTransactions; // late transaction equal on new tansaction
                            Program.blockchain = newBlockChain; // current blockchain equal is new Blockchain 

                        }
                    }
                };

                webSocket.Connect(); // Connect the socket
                webSocket.Send("Hello Server"); // send message on server
                webSocket.Send(JsonConvert.SerializeObject(Program.blockchain)); // send data also server
                webDic.Add(url,webSocket); // add that url and socket on webDic list
            }
        }
           

💣Create a Send Method:

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
        
 💣Create a BroadCast Method for send that all data in the system:
         
          public void BroadCast(string data)
        {
            foreach(var item in webDic)
            {
                item.Value.Send(data);
            }
        }
        
 
 💣Create a Get Servers method: [Get all servers available]
 
            public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach(var item in webDic)
            {
                servers.Add(item.Key);
            }
         
            return servers;

        }
 
 
 💣Last part is create a close method: [close the websocket]
 
            public void Close()
        {
            foreach(var item in webDic)
            {
                item.Value.Close();
               
            }
        }
 
 
 🥇Woow! You are create a first P2P blockchain with .NET Congratssss!!!! 
 
OUTPUT: 


 
![Screenshot 2021-10-19 154154](https://user-images.githubusercontent.com/75094927/137910114-3bb6d92c-7ac5-40da-b392-73ed8e2faf97.png)
