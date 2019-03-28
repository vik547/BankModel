using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Data
{
    public class TransactionInfo
    {
        public int AccountId { private set; get; }

        public DateTime TrancationTime { set; get; }
        public TransactionType TransactionType { set; get; }

        public int Value { private set; get; }

        public int Balance { get; set; }

      

        public TransactionInfo(int id, TransactionType transactionType, int amount)
        {
            this.AccountId = id;
            this.TransactionType = transactionType;
            this.Value = amount;

        }

        public override string ToString()
        {
            return string.Format("{0}   {1} {2} {3} {4} ", AccountId, TrancationTime.ToString("dd/MMM/yyyy hh:mm:ss"), TransactionType.ToString(), Value, Balance);
        }

       
    }
    public enum TransactionType
    {
        Credit,
        Debit
    }
}
