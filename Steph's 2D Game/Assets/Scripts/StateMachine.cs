using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class StateMachine
    {
        private IState _currentState = null;
        private Dictionary<IState, List<Transition>> states = new Dictionary<IState, List<Transition>>();
        
        
        public void AddState(IState state)
        {
            if (state == null) return;
            if(states.ContainsKey(state))
                return;
            states.Add(state, new List<Transition>());
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (from == null || to == null) return;
            states[from].Add(new Transition(from, to, condition));
        }
        
        public void AddTransition(IState to, Func<bool> condition, params IState[] from)
        {
            
            foreach (var f in from)
            {
                AddTransition(f, to, condition);
            }
        }

        public void ChangeState(IState newState)
        {
            if (_currentState != newState)
            {
                _currentState?.OnExit();
                newState.OnEnter();
                _currentState = newState;
            }
        }
        
        public void Update()
        {
            if(_currentState!=null)
                Debug.Log($"Current State: {_currentState.GetType().Name}");
            else Debug.Log("Null State");
            var transitions = states[_currentState];
            foreach (var transition in transitions)
            {
                if (transition.Condition())
                {
                    Debug.Log("Changing to: " + transition.to.GetType());
                    ChangeState(transition.to);
                    return;
                }
            }
            _currentState.Tick();
        }

        public void FixedUpdate()
        {
            _currentState?.FixedTick();
        }
        
        
        public class Transition
        {
            public IState from, to;
            public Func<bool> Condition;

            public Transition(IState @from, IState to, Func<bool> condition)
            {
                this.from = @from;
                this.to = to;
                Condition = condition;
            }
        }
    }
}