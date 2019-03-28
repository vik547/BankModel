using BankApplicationDataFlow.Actions;
using BankApplicationDataFlow.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApplicationDataFlow
{
    class Program
    {
        static void Main(string[] args)
        {

            CreateAccount(0,"Vikram");
            CreateAccount(1, "Acc2");
            CreateAccount(2, "Acc3");
            CreateAccount(3, "Acc4");
            CreateAccount(4, "Acc5");
           

            Thread.Sleep(1000);

            AccountOperations(0);
            AccountOperations(1);
            AccountOperations(2);
            AccountOperations(3);
            AccountOperations(4);



            Console.ReadLine();

            for (int i = 0; i < 1; i++)
            {
                AccountOperation ao = new AccountOperation()
                {
                    Account = new Data.Account(i, ""),
                    OperationType = OperationType.ListOfTransactions,
                };
                AccountOperationAction.AccountOperation.Send(ao);

            }


            Console.ReadLine();

        }

        private static void AccountOperations(int j)
        {
           new Task(delegate() 
           {
             

               for (int i = 0; i < 1000; i++)
               {
                   var r = new Random();
                   AccountOperation ao = new AccountOperation()
                   {
                       Account = new Data.Account(j, ""),
                       OperationType = OperationType.Credit,
                       TransactionAmount = 10
                   };
                   Task.Delay(r.Next(100));
                   AccountOperationAction.AccountOperation.Send(ao);
                   
               }
             
           }).Start();

            new Task(delegate ()
            {
                for (int i = 0; i < 1000; i++)
                {
                    var r = new Random();
                    AccountOperation ao = new AccountOperation()
                    {
                        Account = new Data.Account(j, ""),
                        OperationType = OperationType.Debit,
                        TransactionAmount = 10
                    };
                    Task.Delay(r.Next(100));
                    AccountOperationAction.AccountOperation.Send(ao);
                   // Thread.Sleep(10);
                }
            }).Start();
            //Thread.Sleep(120);
        }

        public static void CreateAccount(int id,string name)
        {
            AccountOperation ao = new AccountOperation()
            {
                Account = new Data.Account(id,name),
                OperationType = OperationType.NewAccount
            };

            AccountOperationAction.AccountOperation.Send(ao);
        }
    }


}


