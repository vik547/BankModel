using BankApplicationDataFlow.Data;
using BankApplicationDataFlow.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Actions
{
    public class AccountOperationAction: Action
    {
        public static AccountOperationAction AccountOperation = new AccountOperationAction();

        ConcurrentDictionary<int,AccountDetail> activeAccounts = new ConcurrentDictionary<int,AccountDetail>();

        public void Handle(Message message)
        {
            AccountOperation accountOperation = message as AccountOperation;
            switch (accountOperation.OperationType)
            {
                case OperationType.NewAccount:
                    {
                        var ad = new AccountDetail();

                        var a = ad.AccountAction.ProceesSync(new AccountMessage() { Account = accountOperation.Account, MessageType = AccountMessage.AccountMessageType.AddAccount });
                        if (a != null)
                        {

                            var am = a as AccountMessage;
                           OutputAction.Action.SendSync(new Output() { Message = "Account Opened , ID : " + am.Account.Id + " Name :" + am.Account.Name });
                            activeAccounts.GetOrAdd(am.Account.Id, ad);
                        }
                        break;
                    }
                case OperationType.Credit:
                    {
                        var ad = activeAccounts[accountOperation.Account.Id];

                        var tr = new TransactionInfoMessage();
                        tr.Account = GetAccountMessage(ad,accountOperation).Account;
                        
                            tr.TransactionInfo = new Data.TransactionInfo(tr.Account.Id, Data.TransactionType.Credit, accountOperation.TransactionAmount);
                            var accountMessage = ad.TransactionAction.ProceesSync(tr);
                            DoTransactionReconcile(ad,tr.Account);
                        
                        break;
                    }
                case OperationType.Debit:
                    {
                        var ad = activeAccounts[accountOperation.Account.Id];

                        var tr = new TransactionInfoMessage();
                        tr.Account = GetAccountMessage(ad, accountOperation).Account;
                      
                            tr.TransactionInfo = new Data.TransactionInfo(tr.Account.Id, Data.TransactionType.Debit, accountOperation.TransactionAmount);
                            var accountMessage = ad.TransactionAction.ProceesSync(tr);

                            DoTransactionReconcile(ad ,tr.Account);
                       
                        break;
                    }
                case OperationType.BalanceInfo:
                    var aBalInfo = activeAccounts[accountOperation.Account.Id].AccountAction.ProcessMessage(new AccountMessage() { Account = accountOperation.Account, MessageType = AccountMessage.AccountMessageType.GetAccountInfo }) as AccountMessage;
                    if (aBalInfo != null && aBalInfo.Account == null)
                       OutputAction.Action.Send(new Output() { Message = string.Format("Account with ID : {0} Current balance {1}", aBalInfo.Account.Id, aBalInfo.Account.CurrentBalance) });
                  
                    break;
                case OperationType.ListOfTransactions:
                    {
                        var acc = activeAccounts[accountOperation.Account.Id].AccountAction.ProcessMessage(new AccountMessage() { Account = accountOperation.Account, MessageType = AccountMessage.AccountMessageType.GetAccountInfo }) as AccountMessage;
                        if (acc != null && acc.Account != null)
                        {
                            activeAccounts[accountOperation.Account.Id].TransactionAction.Send(new TransactionInfoMessage() { Account = acc.Account, Operation = TransactionInfoMessage.TransactionOperation.Statement });
                        }
                        
                    }
                    break;
                case OperationType.UpdateBalance:
                    activeAccounts[accountOperation.Account.Id].AccountAction.Send(new AccountMessage() { Account = accountOperation.Account, MessageType = AccountMessage.AccountMessageType.UpdateBalance });
                    break;
                default:
                    break;
            }
        }

        private AccountMessage GetAccountMessage(AccountDetail ad, AccountOperation accountOperation)
        {
            return ad.AccountAction.ProcessMessage(new AccountMessage() { Account = accountOperation.Account, MessageType = AccountMessage.AccountMessageType.GetAccountInfo }) as AccountMessage;
        }

        public void DoTransactionReconcile(AccountDetail ad, Account a)
        {
             var am = ad.AccountAction.ProcessMessage(new AccountMessage() { Account = a, MessageType = AccountMessage.AccountMessageType.GetAccountInfo }) as AccountMessage;
            if (am != null && am.Account != null)
            {
               ad.TransactionAction.Send(new TransactionInfoMessage() { Account = am.Account, Operation = TransactionInfoMessage.TransactionOperation.Reconcile });
            }
            else
            {
                OutputAction.Action.Send(new Output() { Message = "Account ID : " +a.Id + " Does not exists" });
            }

        }

    }
}
