using System;

namespace AI.FluentFSM.Runtime.Transitions
{
    internal interface ITransition
    {
        Type To { get; }
        internal Type From { get; }
        bool CanTranslate(Type currentState);
    }
}