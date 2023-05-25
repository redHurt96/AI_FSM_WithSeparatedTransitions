namespace AI.FluentFSM.Runtime.States
{
    public abstract class State<TContext> : IState
    {
        protected readonly TContext _context;
        
        protected State(TContext context) => 
            _context = context;
    }
}