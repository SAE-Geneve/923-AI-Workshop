using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    // ReSharper disable once InconsistentNaming
    public class FSM_StatePatrol : FSM_IState
    {

        private List<Transform> _wayPoints;
        private int _wayPointIdx;

        private AIAgentTank _tank;

        public FSM_StatePatrol(AIAgentTank tank)
        {
            _tank = tank;
            _wayPoints = _tank.WayPoints;
        }
        
        public void OnUpdate()
        {
            Debug.Log("Patrol tick !");

            _tank.NavMeshAgent.destination = _wayPoints[_wayPointIdx].position;
            if (_tank.NavMeshAgent.remainingDistance < 1f)
            {
                _wayPointIdx = Random.Range(0, _wayPoints.Count);
            }

        }

        public void OnEnter()
        {
            Debug.Log("Patrol enter state !");
        }

        public void OnExit()
        {
            Debug.Log("Patrol exit state !");
        }
    }
}
