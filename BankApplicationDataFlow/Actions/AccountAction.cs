using BankApplicationDataFlow.Data;
using BankApplicationDataFlow.Messages;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Actions
{
    public class AccountAction: Action
    {
        private Account account;

        public void Handle(Message accountMessage)
        {
            var m = accountMessage as AccountMessage;
            switch (m.MessageType)
            {
                case AccountMessage.AccountMessageType.UpdateBalance:
                    {
                        
                        account = m.Account;
                    }
                 
                    break;
                default:
                    break;
            }
        }

        public  Message ProcessMessage(Message message)
        {
            var m = message as AccountMessage;
            switch (m.MessageType)
            {
                case AccountMessage.AccountMessageType.GetAccountInfo:
                    {

                        return new AccountMessage() { Account = account };
                    }
                 
                default:
                    break;
            }
            return null;
        }

        public  Message ProcessMessageSync(Message message)
        {
            var m = message as AccountMessage;
            switch (m.MessageType)
            {
                case AccountMessage.AccountMessageType.AddAccount:
                    {
                        account = m.Account;
                       OutputAction.Action.Send(new Output() { Message = string.Format("Account with Name {0} is active, Account ID : {1}", account.Name, account.Id) });
                    }
                    return new AccountMessage() { Account = account };
                default:
                    break;
            }
            return null;
        }
      
    }
}
