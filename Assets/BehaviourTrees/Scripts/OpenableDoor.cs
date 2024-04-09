using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTrees
{
    public class OpenableDoor : MonoBehaviour
    {

        private NavMeshObstacle _navMeshObstacle;
        private MeshRenderer _meshRenderer;

        public bool IsLocked => _isLocked;
        [SerializeField] private bool _isLocked;

        private void Start()
        {
            if(!TryGetComponent<NavMeshObstacle>(out _navMeshObstacle))
                Debug.LogError("No component _navMeshObstacle available.");
            if(!TryGetComponent<MeshRenderer>(out _meshRenderer))
                Debug.LogError("No component _meshRenderer available.");
        }

        public void Open()
        {
            _navMeshObstacle.enabled = false;
            _meshRenderer.enabled = false;
        }
    }
}
