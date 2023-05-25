using System;
using System.Collections.Generic;
using AI.FluentFSM.Runtime.States;
using AI.FluentFSM.Runtime.Transitions;
using UnityEngine;

namespace AI.FluentFSM.Runtime.Machine
{
    public class StateMachine
    {
        private IState Current => States[CurrentStateType];
        private ITransition[] CurrentTransitions => Transitions[CurrentStateType];
        
        internal Type CurrentStateType;
        internal Dictionary<Type, IState> States;
        internal string Name;
        internal GameObject Reference;
        internal Dictionary<Type, ITransition[]> Transitions;

        public void Run() => 
            EnterCurrent();

        public void Update()
        {
            foreach (ITransition transition in CurrentTransitions)
            {
                if (transition.CanTranslate(CurrentStateType))
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
            if (Current is IUpdateState updatable)
                updatable.OnUpdate();
        }
        
        private void ExitCurrent()
        {
            if (Current is IExitState exitState)
                exitState.OnExit();   
        }
        
        private void SelectNext(ITransition byTransition)
        {
            Debug.Log($"<b>{Name}</b>: {CurrentStateType.Name} -> <color=green>{byTransition.To.Name}</color>", Reference);
            
            CurrentStateType = byTransition.To;
        }
        
        private void EnterCurrent()
        {
            if (Current is IEnterState enterState)
                enterState.OnEnter();
        }
    }
}
