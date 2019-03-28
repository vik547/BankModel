using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApplicationDataFlow.Data
{
   public class Account
    {

        public int Id { get; set; }

        public string Name { get; set; }


        private int _currentBalance = 0;

        private int l = 0;
        /// <summary>
        /// Deposite would be added as 1st transaction
        /// </summary>
        /// 
        public int CurrentBalance {
            get
            {
                int peek;
                if (balanceUpdates.TryPeek(out peek))
                {
                    Task.Delay(10);
                    return CurrentBalance;
                }
                return _currentBalance;
            }
                set
                    {
                     _currentBalance  =value;
                }
            }


        public Account(int id, string name)
        {
            Id = id;
            Name = name;

        }
        public Account(string name)
        {
            Name = name;

        }

        private ConcurrentQueue<int> balanceUpdates = new ConcurrentQueue<int>();
        public void UpdateBalance(int neValue)
        {
            balanceUpdates.Enqueue(neValue);
            if (0 == Interlocked.Exchange(ref l, 1))
            {
                int result;
               while(balanceUpdates.TryDequeue(out result))
                {
                    _currentBalance += result;
                }
                Interlocked.Exchange(ref l, 0);
            }
            
        }

    }
}
