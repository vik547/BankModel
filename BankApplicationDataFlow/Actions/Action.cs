using BankApplicationDataFlow.Messages;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

public abstract class Action
{
    private ConcurrentExclusiveSchedulerPair cesp = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default);

    public ActionBlock<Message> _action;
    private ActionBlock<Message> _action;
    private ActionBlock<Message> _actionSync;

    private TransformBlock<Message, Message> _transform;
    private TransformBlock<Message, Message> _transformSync;

    public Action()
    {
        _action = new ActionBlock<Message>(message =>
        {
            HandleMessage(message);
        }, new ExecutionDataflowBlockOptions() { TaskScheduler = this.ConcurrentScheduler, MaxDegreeOfParallelism = 10 });

        _actionSync = new ActionBlock<Message>(message =>
        {
            HandleMessage(message);
        }, new ExecutionDataflowBlockOptions() { TaskScheduler = this.ExclusiveScheduler });

        _transform = new TransformBlock<Message, Message>(t => HandleProcessMessage(t), new ExecutionDataflowBlockOptions() { TaskScheduler = ConcurrentScheduler, MaxDegreeOfParallelism =10 });
        _transformSync = new TransformBlock<Message, Message>(t => HandleProcessMessageSync(t), new ExecutionDataflowBlockOptions() { TaskScheduler = ExclusiveScheduler});
    }

    public TaskScheduler ConcurrentScheduler
    {
        get
        {
            return cesp.ConcurrentScheduler;
        }
    }

    public Message HandleProcessMessage(Message message)
    {
        dynamic self = this;
        dynamic mess = message;
        return self.ProcessMessageSync(mess as Message);
    }



    public Message HandleProcessMessageSync(Message message)
    {
        dynamic self = this;
        dynamic mess = message;
        return self.ProcessMessageSync(mess as Message);

    }
    

    public TaskScheduler ExclusiveScheduler
    {
        get
        {
            return cesp.ExclusiveScheduler;
        }
    }


    protected void HandleMessage(Message message)
    {
        dynamic self = this;
        dynamic mess = message;
        self.Handle(mess);
    }

    public void Send(Message message)
    {

        _action.Post(message);
    }

    public void SendSync(Message message)
    {
        _actionSync.Post(message);
    }

    public Message Procees(Message message)
    {
         _transform.Post(message);
        return _transform.Receive();
    }

    public Message ProceesSync(Message message)
    {
        _transformSync.Post(message);
       
        return _transformSync.Receive();
    }
}