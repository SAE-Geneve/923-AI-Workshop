using System;

namespace FSM
{
    // ReSharper disable once InconsistentNaming
    public interface FSM_IState
    {
        void OnUpdate();
        void OnEnter();
        void OnExit();
        
        // Transitions in state machine
        
    }
}
