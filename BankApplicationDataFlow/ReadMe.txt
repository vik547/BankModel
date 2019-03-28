Bank Account model is based on TPL Dataflow Action Blocks
https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library

This model for all actions uses ConcurrentExclusiveSchedulerPair. This scheduler pair is able to action concurrently until exclusive tasks are queued.
https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.concurrentexclusiveschedulerpair?view=netframework-4.7.2 


In my solution there one Base Action class which has 
	- 1 ConcurrentExclusiveSchedulerPair :-  which is used by all the action blocks ( and TransformBlock) to schedule tasks
	- ActionBlock (_action and _actionSync) to push messages on concurrent and Exclusive Schedulers respectively.
	- TransformBlock(_transform and _transformSync) to process message and get response back from concurrent and Exclusive Schedulers respectively.

On a high level Actions act on the messages and each message contains Data and type of action to perform.

Data
	Account :- Contains basic account info (id , name and current balance)
	TransactionInfo :- Captures the details of one transaction.
	AccountDetails :- Denotes one bank account, it contains Actions to act on it's specific account info and account transactions.

Messages :- 
	Meessage :- Base message class.
	AccountMessage :- Denotes a message to act on a Account. Actions can be NewAccount, GetAccountInfo,  UpdateBalance,
	TransactionInfoMessage :- Denotes Message for one Transactions , it Contains object of Account , transactionInfo and Type of operation (Transaction, Reconciliation and List of transactions).
	AccountOperation :- Denotes one message for action on the account. Account Operations can be :-   Credit, Debit, BalanceInfo, ListOfTransactions, NewAccount, UpdateBalance  (Used Internally).
	OutpuutMessage :- Message to print on the console.

Actions :-
	AccountAction :- Denotes one bank account, And is able to perform below actions
		UpdateBalance :- (This is always called using the ExclusiveScheduler)
		GetAccountInfo :- Is called using concurrent scheduler.
	TransactionAction :- Denotes all the transactions for one bank account
		Reconcile and Statement are always called concurrently 
		Transactions is always called using Exclusive scheduler.
	AccountOperations :- Denotes a front of bank branch, where each account holder can act concurrenly.
		All actions are scheduled on concurrent scheduler of AccountOperation object.


