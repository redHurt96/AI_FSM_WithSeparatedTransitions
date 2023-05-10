using System;

namespace AI.FluentFiniteStateMachine
{
    public class Transition : ITransition
    {
        public bool CanTranslate(Type currentState) => 
            _from == currentState && _conditionMethod();

        public Type To { get; }

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