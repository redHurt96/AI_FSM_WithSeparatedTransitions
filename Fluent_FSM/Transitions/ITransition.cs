using System;

namespace AI.FluentFiniteStateMachine
{
    public interface ITransition
    {
        bool CanTranslate(Type currentState);
        Type To { get; }
    }
}