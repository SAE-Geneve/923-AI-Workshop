using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTrees
{
    public class OpenableDoor : MonoBehaviour
    {

        [SerializeField] private NavMeshObstacle _navMeshObstacle;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private bool _isLocked;

        public bool IsLocked => _isLocked;
        public NavMeshObstacle NavMeshObstacle => _navMeshObstacle;

        public void Open()
        {
            _navMeshObstacle.enabled = false;
            _meshRenderer.enabled = false;
        }
    }
}
