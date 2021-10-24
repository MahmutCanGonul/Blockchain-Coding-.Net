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
       
      


OUTPUT: 


 
![Screenshot 2021-10-19 154154](https://user-images.githubusercontent.com/75094927/137910114-3bb6d92c-7ac5-40da-b392-73ed8e2faf97.png)
