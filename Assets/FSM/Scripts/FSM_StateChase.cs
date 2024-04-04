namespace FSM
{
    // ReSharper disable once InconsistentNaming
    public class FSM_StateChase : FSM_IState
    {

        private AIAgentTank _tank;

        public FSM_StateChase(AIAgentTank tank)
        {
            _tank = tank;
        }
        
        public void OnUpdate()
        {
            if(_tank.Target != null)
                _tank.NavMeshAgent.destination = _tank.Target.position;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}
