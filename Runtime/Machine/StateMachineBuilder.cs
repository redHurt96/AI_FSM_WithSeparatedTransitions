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
        private StateMachine _stateMachine;
        private IState _originState;

        private readonly List<IState> _states = new();
        private readonly List<ITransition> _transitions = new();
        private string _name;
        private GameObject _gameObjectReference;

        public StateMachineBuilder SelectFirstState<T>() where T : IState
        {
            _originState = _states.First(
                x => x.GetType() == typeof(T) 
                     || x.GetType().IsSubclassOf(typeof(T)));
            return this;
        }

        public StateMachineBuilder WithState(IState state, Func<bool> canAddState)
        {
            if (canAddState.Invoke())
                WithState(state);

            return this;
        }
        
        public StateMachineBuilder WithState(IState state)
        {
            if (_states.Exists(x => x.GetType() == state.GetType()) || _originState != null  && _originState.GetType() == state.GetType())
                throw new($"State with type {state.GetType().Name} already contains in state machine!");
            
            _states.Add(state);
            return this;
        }

        public StateMachineBuilder WithTransition<TFrom, TTo>(Func<bool> condition)
            where TFrom : IState
            where TTo : IState
        {
            _transitions.Add(new Transition(typeof(TFrom), typeof(TTo), condition));
            return this;
        }
        
        [Obsolete]
        public StateMachineBuilder WithTransition(Type fromState, Type toState, Func<bool> condition)
        {
            _transitions.Add(new Transition(fromState, toState, condition));
            return this;
        }

        public StateMachineBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public StateMachineBuilder WithGameObjectReference(GameObject gameObject)
        {
            _gameObjectReference = gameObject;
            return this;
        }

        public StateMachine Build()
        {
            _stateMachine = new();
            SetupStates();
            SetupTransitions();

            if (!string.IsNullOrEmpty(_name))
                _stateMachine.Name = _name;
            
            if (_gameObjectReference != null)
                _stateMachine.Reference = _gameObjectReference;
            
            return _stateMachine;
        }

        private void SetupStates()
        {
            _stateMachine.States = _states
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