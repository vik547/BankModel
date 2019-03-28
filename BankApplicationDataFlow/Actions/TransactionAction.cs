using BankApplicationDataFlow.Data;
using BankApplicationDataFlow.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BankApplicationDataFlow.Actions
{
    public class TransactionAction : Action
    {

        protected ConcurrentDictionary<Account, ConcurrentStack<TransactionInfo>> _transactionMap = new ConcurrentDictionary<Account, ConcurrentStack<TransactionInfo>>();

        private ConcurrentStack<TransactionInfo> transactions = new ConcurrentStack<TransactionInfo>();
        public void Handle(TransactionInfoMessage message)
        {
            var m = message as TransactionInfoMessage;

            switch (m.Operation)
            {
                case TransactionInfoMessage.TransactionOperation.Reconcile:
                    {
                        TransactionInfo info;
                        transactions.TryPeek(out info);
                        if (info.Balance == m.Account.CurrentBalance)
                        {
                            OutputAction.Action.Send(new Output() { Message = "Account with ID :" + m.Account.Id + " current balance reconciles" });
                        }
                        else
                            OutputAction.Action.Send(new Output() { Message = "Account with ID :" + m.Account.Id + " current balance DO NOT reconciles" });

                    }
                    break;

                case TransactionInfoMessage.TransactionOperation.Statement:
                    {
                      
                            var statmentlist = transactions.ToList();


                        OutputAction.Action.SendSync(new Output() { Message = "Statement for Account ID :" + m.Account.Id });
                        statmentlist.ForEach(s => 
                        {
                            OutputAction.Action.SendSync(new Output() { Message = s.ToString() });
                         });
                            
                      

                    }
                    break;
                default:
                    break;
            }
        }

        public Message ProcessMessageSync(Message message)
        {
            var m = message as TransactionInfoMessage;

            switch (m.Operation)
            {
                case TransactionInfoMessage.TransactionOperation.Transaction:
                    {
                        int newBalance = m.Account.CurrentBalance + (m.TransactionInfo.TransactionType == TransactionType.Credit ? m.TransactionInfo.Value : (-1 * m.TransactionInfo.Value));
                                                
                            var account = m.Account;

                             account.CurrentBalance = newBalance;

                            AccountOperation ao = new AccountOperation() { Account = account, OperationType = OperationType.UpdateBalance };
                            AccountOperationAction.AccountOperation.SendSync(ao);

                            
                            m.TransactionInfo.Balance = newBalance;
                            m.TransactionInfo.TrancationTime = DateTime.Now;
                          

                            transactions.Push(m.TransactionInfo);

                            OutputAction.Action.Send(new Output() { Message = String.Format("Balance for Account ID : {0} for {1} amount {2}  Total Balance : {3}", m.Account.Id, m.TransactionInfo.TransactionType.ToString(), m.TransactionInfo.Value, m.TransactionInfo.Balance) });

                        return m;
                    }
                    

                default:
                    break;
            }
            return null;
        }

        public Message ProcessMessage(Message message)
        {
            return message;
        }


    }
}
