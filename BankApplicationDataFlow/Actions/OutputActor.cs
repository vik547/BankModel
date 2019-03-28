using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApplicationDataFlow.Messages;

namespace BankApplicationDataFlow.Actions
{
   public class OutputAction : Action
    {
        public static OutputAction Action = new OutputAction();

        public void Handle(Output output)
        {
            Console.WriteLine(output.Message);
        }
      
    }
}
