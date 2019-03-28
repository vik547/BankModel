using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Messages
{
    public class AccountMessage : Message
    {
        public BankApplicationDataFlow.Data.Account Account { get; set; }

        public AccountMessageType MessageType { get; set; }
        public enum AccountMessageType
        {
            NewAccount,
            GetAccountInfo,
            AddAccount,
            UpdateBalance,
        }

    }


}
