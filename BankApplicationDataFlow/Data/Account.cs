namespace BankApplicationDataFlow.Data
{
    public class Account
    {

        public int Id { get; set; }

        public string Name { get; set; }


        
       
       
        public int CurrentBalance { get;set; }


        public Account(int id, string name)
        {
            Id = id;
            Name = name;

        }
        public Account(string name)
        {
            Name = name;

        }

        

    }
}
