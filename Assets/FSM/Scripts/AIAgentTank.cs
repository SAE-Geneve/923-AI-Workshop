using System;
using System.Collections.Generic;
using FSM;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class AIAgentTank : MonoBehaviour
    {

        private FSM_StateMachine _stateMachine;

        private FSM_StatePatrol _patrolState;
        private FSM_StateChase _chaseState;
        private FSM_StateChase _chaseState2;

        private NavMeshAgent _navMeshAgent;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        [SerializeField] private List<Transform> _wayPoints;
        public List<Transform> WayPoints => _wayPoints;

        private Transform _target;
        public Transform Target => _target;

        public void Start()
        {

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _target = GameObject.FindWithTag("Player").transform;
            
            _stateMachine = new FSM_StateMachine();
            _patrolState = new FSM_StatePatrol(this);
            _chaseState = new FSM_StateChase(this);
            
            _stateMachine.AddTransition(_patrolState, _chaseState, () => Vector3.Distance(transform.position, _target.position) < 5f);
            _stateMachine.AddTransition(_chaseState, _patrolState, () => Vector3.Distance(transform.position, _target.position) >= 5f);
            
            _stateMachine.ChangeState(_patrolState);
            
        }

        private bool SufficientDistance()
        {
            return Vector3.Distance(transform.position, _target.position) < 5f;
        }

        public void Update()
        {
            _stateMachine.UpdateState();
        }
        
        
    }
