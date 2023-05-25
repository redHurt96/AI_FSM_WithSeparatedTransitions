using System.Linq;
using AI.FluentFSM.Runtime.States;
using AI.FluentFSM.Runtime.Transitions;
using UnityEngine;

namespace AI.FluentFSM.Runtime.Machine
{
    public class StateMachineBuilder
    {
        private readonly StateMachine _stateMachine;
        private ITransition[] _transitions;

        public StateMachineBuilder() => 
            _stateMachine = new StateMachine();

        public StateMachineBuilder WithStates(IState startState, params IState[] states)
        {
            _stateMachine.States = states
                .Concat(new[] { startState })
                .ToDictionary(x => x.GetType(), y => y);
            _stateMachine.CurrentStateType = startState.GetType();
            
            return this;
        }

        public StateMachineBuilder WithTransitions(params ITransition[] transitions)
        {
            _transitions = transitions;
            return this;
        }

        public StateMachineBuilder WithName(string name)
        {
            _stateMachine.Name = name;
            return this;
        }

        public StateMachineBuilder WithGameObjectReference(GameObject gameObject)
        {
            _stateMachine.Reference = gameObject;
            return this;
        }

        public StateMachine Build()
        {
            SetupTransitions();
            return _stateMachine;
        }

        private void SetupTransitions() =>
            _stateMachine.Transitions = _transitions
                .Select(x => x.From)
                .Distinct()
                .ToDictionary(
                    x => x,
                    y => _transitions
                        .Where(x => x.From == y)
                        .ToArray());
    }
}