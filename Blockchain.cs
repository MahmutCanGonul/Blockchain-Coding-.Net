using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainCoding
{
    public class Blockchain
    {
        public IList<Transaction> pendingTransactions = new List<Transaction>(); // Bekletilen Transactionlar tutulmaktadir
        public IList<Block> chain;
        public int difficulty { get; set; } = 2;
        public int Reward { get; set; } = 1;


        public Blockchain()
        {
          
        }

        public void InitializeBlock() // Start Chain
        {
            chain = new List<Block>();
            AddGenesisBlock();
        }


        public Block CreateGenesisBlock() //Creat Chain
        {
            Block block = new Block(DateTime.Now, null, pendingTransactions);
            block.Mine(this.difficulty);
            pendingTransactions = new List<Transaction>();
            return block;
        }

        public void AddGenesisBlock() // Add Chain
        {
            chain.Add(CreateGenesisBlock());
        }

        public Block GetLastBlock() // Get Last Chain
        {
            return chain[chain.Count - 1];
        }


        public void AddBlock(Block block)
        {
            Block latestBlock = GetLastBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            block.Mine(this.difficulty); // if I use chain count mining time will be increase and dependent on chain count 
            chain.Add(block);

        }

        public void CreateTransaction(Transaction transaction) // We are create Transaction Here
        {
            pendingTransactions.Add(transaction);

        }

        public void ProcessPendingTransaction(string minerAddress)
        {
            CreateTransaction(new Transaction(null, minerAddress, Reward)); // Minera odul verilmekte burada
            Block block = new Block(DateTime.Now,GetLastBlock().Hash,pendingTransactions); // Bloga transactioni yazdik
            AddBlock(block); // ve burda ekledik
            pendingTransactions = new List<Transaction>(); // Dolan Transactioni Boşalt yani yeniden baslat
            

        }
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

        public int GetBalanceAmount(string address) // Get Balance from to address
        {
            int balance = 0;
           for(int i=0; i < chain.Count; i++)
            {
                for(int j=0; j < chain[i].Transactions.Count; j++)
                {
                    var transaction = chain[i].Transactions[j];
                    if(transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if(transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }

                }
            }

            return balance;

        }

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

        

    }
}
