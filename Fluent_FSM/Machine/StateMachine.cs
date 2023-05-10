using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.FluentFiniteStateMachine
{
    public class StateMachine
    {
        private IState _current => _states[_currentStateType];
        
        private Type _currentStateType;
        private Dictionary<Type, IState> _states;
        private ITransition[] _transitions;

        public StateMachine AddStates(IState startState, params IState[] states)
        {
            _states = states
                .Concat(new[] { startState })
                .ToDictionary(x => x.GetType(), y => y);
            
            _currentStateType = startState.GetType();
            
            return this;
        }

        public StateMachine AddTransitions(params ITransition[] transitions)
        {
            _transitions = transitions;
            return this;
        }

        public void Update()
        {
            foreach (ITransition transition in _transitions)
            {
                if (transition.CanTranslate(_currentStateType))
                {
                    ChangeState(transition);
                    break;
                }
            }
            
            UpdateCurrentState();
        }
        
        private void ChangeState(ITransition transition)
        {
            ExitCurrent();
            SelectNext(transition);
            EnterCurrent();
        }
        
        private void UpdateCurrentState()
        {
            if (_current is IUpdateState updatable)
                updatable.OnUpdate();
        }
        
        private void ExitCurrent()
        {
            if (_current is IExitState exitState)
                exitState.OnExit();   
        }
        
        private void SelectNext(ITransition byTransition)
        {
            Debug.Log($"{_currentStateType.Name} -> <color=green>{byTransition.To.Name}</color>");
            
            _currentStateType = byTransition.To;
        }
        
        private void EnterCurrent()
        {
            if (_current is IEnterState enterState)
                enterState.OnEnter();
        }
    }
}
