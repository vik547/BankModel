using BankApplicationDataFlow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Messages
{
    public class AccountOperation :Message
    {
        public Account Account { get; set; }

        
        public OperationType OperationType { get; set; }
        

        public int TransactionAmount { get; set; }
    }
    public enum OperationType
    {
        Credit,
        Debit,
        BalanceInfo,
        ListOfTransactions,
        NewAccount,
        UpdateBalance  //Used Internally
    }
}
