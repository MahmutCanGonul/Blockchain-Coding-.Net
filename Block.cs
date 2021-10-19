using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace BlockchainCoding
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeSpan { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions;
        //public string Data { get; set; }
        public int Nonce { get; set; } = 0;
        public Block(DateTime timeSpan,string previousHash, IList<Transaction> transactions)
        {
            TimeSpan = timeSpan;
            PreviousHash = previousHash;
            //Data = data;
            Transactions = transactions;
            //Hash = CalculateHash();
          
        }

        public string CalculateHash()
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] inBytes = Encoding.ASCII.GetBytes($"{TimeSpan}--{PreviousHash ?? ""}--{JsonConvert.SerializeObject(Transactions)}--{Nonce}"); // We take block datas in byte array 
            byte[] outBytes = sHA256.ComputeHash(inBytes); // Calculate has with SHA256 on our in-bytes
            return Convert.ToBase64String(outBytes); // Change byte array on string
        }


        public void Mine(int difficulty) // Proof of Work Logic
        {
            var leadingzeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0,difficulty) != leadingzeros)
            {
                this.Nonce++;
                this.Hash = CalculateHash();
            }
        }


    }
}
