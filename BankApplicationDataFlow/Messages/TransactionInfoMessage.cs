using BankApplicationDataFlow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Messages
{
    public class TransactionInfoMessage : Message
    {
       public Account Account { get; set; }
        public TransactionInfo TransactionInfo { set; get; }

        public TransactionOperation Operation { get; set; }
        public enum TransactionOperation
        {
            Transaction,
            Reconcile,
            Statement
        }

        
    }

   
}
