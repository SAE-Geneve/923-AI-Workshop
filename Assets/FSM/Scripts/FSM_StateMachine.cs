﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace FSM
{
    // ReSharper disable once InconsistentNaming
    public class FSM_StateMachine
    {
        private FSM_IState _currentState;
        private List<FSM_Transition> _transitions = new List<FSM_Transition>();
        
        public void ChangeState(FSM_IState newState)
        {
            // Make the exit instructions
            _currentState?.OnExit();
            
            // if(_currentState != null)
            //     _currentState.OnExit();
            
            // Change the state
            _currentState = newState;
            
            // Execute the first instructions
            _currentState.OnEnter();
            
        }

        public void UpdateState()
        {
            if(_currentState != null)
                _currentState.OnUpdate();
            else
                Debug.LogWarning("no current state !!!! ");
            
            CheckTransitions();
            
        }

        private void CheckTransitions()
        {
            // if found transition in the list
            // 1. where from state == _currentState
            // 2. AND _condition 
            var possibleTransition =_transitions.FirstOrDefault(t => t._from == _currentState && t._condition());
            if (possibleTransition != null)
            {
                // => Change State
                ChangeState(possibleTransition._to);
            }
            
        }

        public void AddTransition(FSM_IState from, FSM_IState to, Func<bool> condition)
        {
            // LIST 
            if(from != null && to != null && condition != null)
            {
                _transitions.Add(new FSM_Transition(from, to, condition));
            }
            
        }
        
    }

    // ReSharper disable once InconsistentNaming
    public class FSM_Transition
    {
        public FSM_IState _from;
        public FSM_IState _to;
        public Func<bool> _condition;

        public FSM_Transition(FSM_IState from, FSM_IState to, Func<bool> condition)
        {
            _from = from;
            _to = to;
            _condition = condition;
        }

    }
    
}
