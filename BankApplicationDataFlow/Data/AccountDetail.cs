using BankApplicationDataFlow.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Data
{
    public class AccountDetail
    {
        public AccountAction AccountAction { get; set; }

        public TransactionAction TransactionAction { get; set; }

        public AccountDetail()
        {
            TransactionAction = new TransactionAction();
            AccountAction = new AccountAction();
        }
    }
}
