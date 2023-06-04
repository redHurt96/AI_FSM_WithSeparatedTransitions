using System;

namespace AI.FluentFSM.Runtime.Transitions
{
    internal class Transition : ITransition
    {
        public Type To { get; }
        
        Type ITransition.From => _from;

        public bool CanTranslate(Type currentState) => 
            _from == currentState && _conditionMethod();

        private readonly Func<bool> _conditionMethod;
        private readonly Type _from;

        public Transition(Type from, Type to, Func<bool> conditionMethod)
        {
            To = to;
            _conditionMethod = conditionMethod;
            _from = from;
        }
    }
}