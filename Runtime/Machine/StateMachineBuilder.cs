using System;
using System.Collections.Generic;
using System.Linq;
using AI.FluentFSM.Runtime.States;
using AI.FluentFSM.Runtime.Transitions;
using UnityEngine;

namespace AI.FluentFSM.Runtime.Machine
{
    public class StateMachineBuilder
    {
        private readonly StateMachine _stateMachine;
        private readonly IState _originState;
        private readonly List<IState> _states = new();
        private readonly List<ITransition> _transitions = new();

        public StateMachineBuilder(IState firstState)
        {
            _originState = firstState;
            _stateMachine = new();
        }

        public StateMachineBuilder WithState(IState state)
        {
            if (_states.Exists(x => x.GetType() == state.GetType()) || _originState != null  && _originState.GetType() == state.GetType())
                throw new($"State with type {state.GetType().Name} already contains in state machine!");
            
            _states.Add(state);
            return this;
        }

        public StateMachineBuilder WithTransition(Type fromState, Type toState, Func<bool> condition)
        {
            _transitions.Add(new Transition(fromState, toState, condition));
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
            SetupStates();
            SetupTransitions();
            return _stateMachine;
        }

        private void SetupStates()
        {
            _stateMachine.States = _states
                .Concat(new[] { _originState })
                .ToDictionary(x => x.GetType(), y => y);
            _stateMachine.CurrentStateType = _originState.GetType();
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